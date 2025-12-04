using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    [Header("Configuracion del Objeto")]
    [SerializeField] private string objectName = "Objeto";
    [SerializeField] private string interactionMessage = "Examinar este objeto";
    [SerializeField] private bool canPickup = false;
    [SerializeField] private Sprite inventoryIcon;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject interactionPrompt; // Presionar E
    [SerializeField] private GameObject highlightSprite;   // Sprite con brillo

    [Header("Eventos")]
    public UnityEvent onInteract;

    private Inventario inventory;

    [SerializeField] private PanelManager panelManager;
    [SerializeField] private string panelNameToShow;

    [Header("Puzzle Requirements")]
    public string requiredPuzzleID = "";         // puzzle que debe estar completo antes de permitir interacción
    public PuzzleBase puzzleToStart;             // puzzle que este objeto activa
   
    // Si es el primer puzzle, permitir prompt desde el inicio
    public bool isFirstPuzzle = false;

    private bool puzzleUnlocked = true;

    // bloqueo de interacción
    private bool usable = true;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventario>();

        // Si este objeto requiere un puzzle previo, NO debe iniciarse usable
        if (!string.IsNullOrEmpty(requiredPuzzleID))
            usable = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (highlightSprite != null)
            highlightSprite.SetActive(false);

        // Si este objeto inicia un puzzle, NO permitir prompt hasta desbloquearlo
        if(puzzleToStart != null && !isFirstPuzzle)
        puzzleUnlocked = false;
    }

    // Llamado desde el Player cuando presiona E
    public void Interact()
    {
        // NO interactuar si está bloqueado
        if (!usable)
            return;

        Debug.Log("Interactuando con: " + objectName);

        if (canPickup && inventory != null)
        {
            inventory.AddItem(objectName, inventoryIcon);
            Destroy(gameObject);
        }
        else
        {
            ShowInteractionMessage();
        }

        onInteract?.Invoke();

        if (panelManager != null && !string.IsNullOrEmpty(panelNameToShow))
        {
            panelManager.ShowPanel(panelNameToShow);
        }

        // Si tiene un puzzle asignado, iniciarlo aquí
        if (puzzleToStart != null)
            puzzleToStart.StartPuzzle();
    }

    void ShowInteractionMessage()
    {
        Debug.Log(interactionMessage);

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble(interactionMessage, transform);
        }
    }

    public void ShowPrompt(bool show)
    {
        // Si el puzzle aún no está desbloqueado, no mostrar prompts
        if (!puzzleUnlocked)
            show = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);

        if (highlightSprite != null)
            highlightSprite.SetActive(show);
    }

    public string GetObjectName() => objectName;
    public string GetInteractionMessage() => interactionMessage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!usable) return;

        if (other.CompareTag("Player"))
            ShowPrompt(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPrompt(false);
            DialogueManager.Instance?.HideBubble();
        }
    }

    // LLAMAR DESDE PuzzleBase.CompletePuzzle()
    public void DisableInteraction()
    {
        usable = false;
        ShowPrompt(false);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Debug.Log("InteractionObject deshabilitado.");
    }

    // LLAMADO DESDE PuzzleBase cuando se termina un puzzle anterior
    public void EnableInteractionFromPuzzle()
    {
        usable = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        Debug.Log("InteractionObject habilitado por puzzle.");
    }

    public void UnlockPuzzlePrompt()
    {
        puzzleUnlocked = true;
        Debug.Log("Prompt desbloqueado para: " + objectName);
    }

}
