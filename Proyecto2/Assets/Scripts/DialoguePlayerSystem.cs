using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguePlayerSystem : MonoBehaviour
{
    public static DialoguePlayerSystem Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Button nextButton;

    private DialogueData activeDialogue;
    private int currentIndex = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData data)
    {
        if (DialoguePersistence.WasSeen(data.dialogueID) && !data.repeatable)
            return;

        activeDialogue = data;
        currentIndex = 0;

        dialoguePanel.SetActive(true);
        dialogueText.text = activeDialogue.lines[currentIndex].text;

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextLine);
    }

    private void NextLine()
    {
        currentIndex++;

        if (currentIndex >= activeDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = activeDialogue.lines[currentIndex].text;
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        DialoguePersistence.WasSeen(activeDialogue.dialogueID);
    }
}
