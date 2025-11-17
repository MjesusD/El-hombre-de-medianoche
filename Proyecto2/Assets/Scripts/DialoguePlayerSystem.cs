using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

        // Bloquear movimiento del jugador
        if (Player.Instance != null)
            Player.Instance.SetCanMove(false);

        subtitlePanel.SetActive(true);
        ShowCurrentLine();
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (Input.GetKeyDown(nextKey))
        {
            NextLine();
        }
    }

    public void NextLine()
    {
        lineIndex++;

        if (lineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    private void ShowCurrentLine()
    {
        subtitleText.text = currentDialogue.lines[lineIndex].text;
    }

    private void EndDialogue()
    {
        isPlaying = false;
        subtitlePanel.SetActive(false);

        // Desbloquear movimiento del jugador
        if (Player.Instance != null)
            Player.Instance.SetCanMove(true);

        // Guardar persistencia opcional
        DialoguePersistence.MarkAsSeen(currentDialogue.dialogueID);

        currentDialogue = null;
    }
}
