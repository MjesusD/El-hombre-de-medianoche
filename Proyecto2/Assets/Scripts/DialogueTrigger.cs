using UnityEngine;

public class InkTrigger : MonoBehaviour
{
    [Header("Ink")]
    public TextAsset inkJSON;

    [Header("Knot Names")]
    public string firstKnot = "start";          // diálogo primera vez
    public string repeatKnot = "start_repeat";  // diálogo repetido

    [Header("Settings")]
    public string dialogueID = "npc_default";   // ID único por NPC
    public KeyCode interactKey = KeyCode.E;

    [Header("Optional Item Requirement")]
    public bool requiresItem = false;
    public string expectedItemName = ""; // si este NPC requiere un ítem

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
        {
            if (inkJSON == null)
            {
                Debug.LogError("[InkTrigger] NO hay inkJSON en " + gameObject.name);
                return;
            }

            // Elegir si mostrar first o repeat
            string knotToUse =
                DialoguePersistence.WasSeen(dialogueID)
                ? repeatKnot
                : firstKnot;

            // --- ADAPTACIÓN IMPORTANTE ---
            // Informar al manager qué trigger activó este diálogo
            InkDialogueManager.Instance.SetCurrentTrigger(this);

            // Si este NPC requiere ítem para avanzar
            if (requiresItem)
            {
                InkDialogueManager.Instance.SetExpectedItem(expectedItemName);
            }

            // Iniciar diálogo
            InkDialogueManager.Instance.StartStory(inkJSON, knotToUse);

            // Marcar como visto
            DialoguePersistence.MarkSeen(dialogueID);
        }
    }

    // --- TRIGGERS ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    // --- ÚTIL PARA OTROS SCRIPTS ---
    public void SetDialogue(string newFirstKnot, string newRepeatKnot = "")
    {
        firstKnot = newFirstKnot;

        if (!string.IsNullOrEmpty(newRepeatKnot))
            repeatKnot = newRepeatKnot;
    }

    public void ResetDialogueSeen()
    {
        PlayerPrefs.SetInt("dialogue_" + dialogueID, 0);
    }

#if UNITY_EDITOR
    [ContextMenu("Reset First Dialogue")]
    public void EditorResetThisDialogue()
    {
        ResetDialogueSeen();
        Debug.Log("Reset diálogo para NPC: " + dialogueID);
    }
#endif


}
