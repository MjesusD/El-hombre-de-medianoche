using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    [Header("Configuración del Objeto")]
    [SerializeField] private string objectName = "Objeto";
    [SerializeField] private string interactionMessage = "Examinar este objeto";
    [SerializeField] private bool canPickup = false;
    [SerializeField] private Sprite inventoryIcon;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject interactionPrompt; // Presionar E
    [SerializeField] private GameObject highlightSprite;   // Sprite con brillo

    [Header("Eventos")]
    public UnityEvent onInteract;

    private inventario inventory;

    void Start()
    {
        inventory = FindAnyObjectByType<inventario>();

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (highlightSprite != null)
            highlightSprite.SetActive(false);
    }

    // Llamado desde el Player cuando presiona E
    public void Interact()
    {
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
        if (interactionPrompt != null)
            interactionPrompt.SetActive(show);

        if (highlightSprite != null)
            highlightSprite.SetActive(show);
    }

    public string GetObjectName() => objectName;
    public string GetInteractionMessage() => interactionMessage;

    private void OnTriggerEnter2D(Collider2D other)
    {
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
}
