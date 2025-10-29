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
    [SerializeField] private GameObject interactionPrompt; //UI "Presiona E"
    [SerializeField] private GameObject highlightSprite; //sprite que brilla al estar cerca

    [Header("Eventos")]
    public UnityEvent onInteract;

    private inventario inventory;

    void Start()
    {
        inventory = FindObjectOfType<inventario>();

        //ocultar elementos visuales al inicio
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        if (highlightSprite != null)
        {
            highlightSprite.SetActive(false);
        }
    }
    //metodo llamado desde Player cuando presiona E
    public void Interact()
    {
        Debug.Log("Interactuando con: " + objectName);

        if (canPickup && inventory != null)
        {
            //agregar al inventario y destruir del mundo
            inventory.AddItem(objectName, inventoryIcon);
            Destroy(gameObject);
        }
        else
        {
            //mostrar mensaje de interaccion
            ShowInteractionMessage();
        }

        //ejecutar eventos personalizados
        onInteract?.Invoke();
    }

    void ShowInteractionMessage()
    {
        //aqui conectarias con tu sistema de dialogos
        Debug.Log(interactionMessage);

        // Ejemplo futuro:
        // DialogueManager.Instance.ShowMessage(interactionMessage);
    }

    //mostrar/ocultar el prompt de "Presiona E"
    public void ShowPrompt(bool show)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(show);
        }

        if (highlightSprite != null)
        {
            highlightSprite.SetActive(show);
        }
    }

    //obtener informacion del objeto
    public string GetObjectName()
    {
        return objectName;
    }

    public string GetInteractionMessage()
    {
        return interactionMessage;
    }
}


