using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MusicalGameManager : PuzzleBase
{
    public static MusicalGameManager Instance;

    [Header("Botones")]
    public MusicalButton buttonPrefab;
    public Transform layoutParent;
    public int totalButtons = 4;

    [Header("Juego")]
    public int sequenceLength = 4;
    public float delayBetweenFlashes = 0.5f;
    public float errorFlashDuration = 0.5f;

    private List<MusicalButton> buttons = new List<MusicalButton>();
    private List<int> sequence = new List<int>();
    private int playerIndex = 0;
    private bool inputEnabled = false;
    private Coroutine sequenceCoroutine;

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
        GenerateSequence();
        PlaySequenceWithDelay(1f);
    }

    void GenerateButtons()
    {
        buttons.Clear();

        for (int i = 0; i < totalButtons; i++)
        {
            MusicalButton mb = Instantiate(buttonPrefab, layoutParent);
            mb.buttonID = i;

            int capturedID = i;
            // Limpiar listeners por si se reinicia el puzzle
            mb.GetComponent<Button>().onClick.RemoveAllListeners();
            mb.GetComponent<Button>().onClick.AddListener(() => mb.OnPlayerClick());

            buttons.Add(mb);
        }
    }

    public void PlaySequenceWithDelay(float delay)
    {
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        sequenceCoroutine = StartCoroutine(_PlaySequenceWithDelay(delay));
    }

    private IEnumerator _PlaySequenceWithDelay(float delay)
    {
        inputEnabled = false;
        playerIndex = 0;
        UpdateButtonsInteractable();

        yield return new WaitForSeconds(delay);

        foreach (int id in sequence)
        {
            buttons[id].Highlight();
            yield return new WaitForSeconds(delayBetweenFlashes + 0.3f);
        }

        inputEnabled = true;
        UpdateButtonsInteractable();
    }

    void GenerateSequence()
    {
        sequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            sequence.Add(Random.Range(0, totalButtons));
        }
    }

    void UpdateButtonsInteractable()
    {
        foreach (var btn in buttons)
            btn.GetComponent<Button>().interactable = inputEnabled;
    }

    public void PlayerPress(int id)
    {
        if (!inputEnabled) return;

        if (id == sequence[playerIndex])
        {
            playerIndex++;

            if (playerIndex >= sequence.Count)
            {
                Debug.Log("Puzzle Completado");
                inputEnabled = false;
                UpdateButtonsInteractable();
                CompletePuzzle();
            }
        }
        else
        {
            Debug.Log("Fallaste. Reproduciendo secuencia nuevamente...");
            StartCoroutine(FlashButtonsError());
        }
    }

    private IEnumerator FlashButtonsError()
    {
        inputEnabled = false;
        UpdateButtonsInteractable();

        // Poner todos los botones en rojo
        foreach (var btn in buttons)
            btn.SetErrorState(true);

        yield return new WaitForSeconds(errorFlashDuration);

        // Volver al color normal
        foreach (var btn in buttons)
            btn.SetErrorState(false);

        yield return new WaitForSeconds(0.3f); // pequeño delay antes de la nueva secuencia

        // Generar secuencia nueva
        GenerateSequence();

        // Reproducir la nueva secuencia
        PlaySequenceWithDelay(0.5f);
    }

}
