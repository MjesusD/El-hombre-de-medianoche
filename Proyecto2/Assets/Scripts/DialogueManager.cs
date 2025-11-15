using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Configuración")]
    public GameObject bubblePrefab;
    public Canvas uiCanvas;

    private GameObject currentBubble;
    private DialogueData currentDialogue;
    private int currentIndex;
    private Transform currentTarget;

    void Awake()
    {
        Instance = this;
    }

    // Mostrar SOLO un mensaje (burbuja actual)
    
    public void ShowBubble(string message, Transform worldTarget)
    {
        if (bubblePrefab == null || uiCanvas == null)
        {
            Debug.LogWarning("Falta prefab o canvas");
            return;
        }

        if (currentBubble != null)
            Destroy(currentBubble);

        currentBubble = Instantiate(bubblePrefab, uiCanvas.transform);

        var bubbleScript = currentBubble.GetComponent<DialogueBubble>();
        bubbleScript.Setup(message, worldTarget);
    }

    public void HideBubble()
    {
        if (currentBubble != null)
            Destroy(currentBubble);
    }

    // ----------------------------------------------
    // Sistema de diálogos completos
    // ----------------------------------------------
    public void StartDialogue(DialogueData data, Transform target)
    {
        if (data == null) return;

        currentDialogue = data;
        currentTarget = target;
        currentIndex = 0;

        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (currentDialogue == null) return;

        // Fin del diálogo
        if (currentIndex >= currentDialogue.lines.Length)
        {
            DialoguePersistence.WasSeen(currentDialogue.dialogueID);
            HideBubble();
            currentDialogue = null;
            return;
        }

        string line = currentDialogue.lines[currentIndex].text;
        currentIndex++;

        ShowBubble(line, currentTarget);
    }

    public bool IsDialoguePlaying()
    {
        return currentDialogue != null;
    }
}
