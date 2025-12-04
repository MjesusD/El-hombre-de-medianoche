using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MusicalGameManager : PuzzleBase
{
    public static MusicalGameManager Instance;

    [Header("Botones")]
    public MusicalButton buttonPrefab;   // prefab del botón
    public Transform layoutParent;       // el contenedor con LayoutGroup
    public int totalButtons = 4;

    [Header("Juego")]
    public int sequenceLength = 4;
    public float delayBetweenFlashes = 0.5f;

    private List<MusicalButton> buttons = new List<MusicalButton>();
    private List<int> sequence = new List<int>();
    private int playerIndex = 0;
    private bool inputEnabled = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshUIFader();
        puzzlePanel.SetActive(false);
        GenerateButtons();
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();
        StartCoroutine(PlaySequence());
    }


    //   GENERAR BOTONES
   
    void GenerateButtons()
    {
        buttons.Clear();

        for (int i = 0; i < totalButtons; i++)
        {
            MusicalButton mb = Instantiate(buttonPrefab, layoutParent);
            mb.buttonID = i;

            // conectar evento OnClick
            mb.GetComponent<Button>().onClick.AddListener(() => mb.OnPlayerClick());

            buttons.Add(mb);
        }
    }

 
    //   SECUENCIA
    
    IEnumerator PlaySequence()
    {
        inputEnabled = false;

        GenerateSequence();

        yield return new WaitForSeconds(1f);

        foreach (int id in sequence)
        {
            buttons[id].Highlight();
            yield return new WaitForSeconds(delayBetweenFlashes);
        }

        inputEnabled = true;
        playerIndex = 0;
    }

    void GenerateSequence()
    {
        sequence.Clear();

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomID = Random.Range(0, totalButtons);
            sequence.Add(randomID);
        }

        playerIndex = 0;
    }


    //   INPUT DEL JUGADOR
  
    public void PlayerPress(int id)
    {
        if (!inputEnabled)
            return;

        if (id == sequence[playerIndex])
        {
            playerIndex++;

            if (playerIndex >= sequence.Count)
            {
                Debug.Log("Puzzle Completado");
                inputEnabled = false;
                CompletePuzzle();
            }
        }
        else
        {
            Debug.Log("Fallaste. Repitiendo secuencia...");
            StartCoroutine(PlaySequence());
        }
    }
}
