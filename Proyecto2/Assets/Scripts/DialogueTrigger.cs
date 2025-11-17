using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    public KeyCode interactKey = KeyCode.E;

    private bool playerNearby = false;

    void Update()
    {
        if (!playerNearby) return;

        // No iniciar si ya hay uno corriendo
        if (DialoguePlayerSystem.Instance != null &&
            DialoguePlayerSystem.Instance.IsPlaying())
        {
            return;
        }

        // Iniciar diálogo al presionar E
        if (Input.GetKeyDown(interactKey))
            TryStartDialogue();
    }

    void TryStartDialogue()
    {
        if (dialogue == null) return;

        // Si el diálogo no es repetible y ya fue visto, no iniciar
        if (!dialogue.repeatable &&
            DialoguePersistence.WasSeen(dialogue.dialogueID))
            return;

        DialoguePlayerSystem.Instance.StartDialogue(dialogue);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
