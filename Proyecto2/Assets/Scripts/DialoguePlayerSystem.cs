using UnityEngine;
using TMPro;

public class DialoguePlayerSystem : MonoBehaviour
{
    public static DialoguePlayerSystem Instance;

    [Header("UI de diálogos")]
    public GameObject subtitlePanel;
    public TextMeshProUGUI subtitleText;

    [Header("Configuración")]
    public KeyCode nextKey = KeyCode.Space;

    private DialogueData currentDialogue;
    private int lineIndex = 0;
    private bool isPlaying = false;

    // Datos del spawn al terminar 
    private GameObject itemToSpawn;
    private Transform spawnPoint;

    public delegate void DialogueEvent();
    public event DialogueEvent OnDialogueFinished;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (subtitlePanel != null)
            subtitlePanel.SetActive(false);
    }

    public bool IsPlaying() => isPlaying;

    public void StartDialogue(DialogueData data)
    {
        if (isPlaying || data == null)
            return;

        currentDialogue = data;
        lineIndex = 0;
        isPlaying = true;

        if (Player.Instance != null)
            Player.Instance.SetCanMove(false);

        subtitlePanel.SetActive(true);
        ShowCurrentLine();
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (Input.GetKeyDown(nextKey))
            NextLine();
    }

    public void NextLine()
    {
        lineIndex++;

        if (lineIndex >= currentDialogue.lines.Length)
            EndDialogue();
        else
            ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        subtitleText.text = currentDialogue.lines[lineIndex].text;
    }

    private void EndDialogue()
    {
        isPlaying = false;
        subtitlePanel.SetActive(false);

        if (Player.Instance != null)
            Player.Instance.SetCanMove(true);

        DialoguePersistence.MarkAsSeen(currentDialogue.dialogueID);

        // Ejecutar spawn si existe 
        if (itemToSpawn != null && spawnPoint != null)
        {
            Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
            itemToSpawn = null;
            spawnPoint = null;
        }

        // Llamar evento global
        OnDialogueFinished?.Invoke();

        currentDialogue = null;
    }

   
    // Asigna un ítem para aparecer al final del diálogo
    
    public void RegisterItemSpawnAfterDialogue(GameObject prefab, Transform point)
    {
        itemToSpawn = prefab;
        spawnPoint = point;
    }
}
