using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    public Transform bubbleAnchor;

    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryStartDialogue();
        }

        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialoguePlaying() &&
            Input.GetKeyDown(KeyCode.Space))
        {
            DialogueManager.Instance.ShowNextLine();
        }
    }

    void TryStartDialogue()
    {
        if (dialogue == null) return;

        // Si ya se vio y no es repetible, no reproducir
        if (!dialogue.repeatable && DialoguePersistence.WasSeen(dialogue.dialogueID))
        {
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogue, bubbleAnchor);
    }
}
