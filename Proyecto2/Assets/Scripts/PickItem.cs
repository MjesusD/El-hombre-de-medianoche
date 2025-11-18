using UnityEngine;

public class PickItem : MonoBehaviour
{
    [Header("Información del Item")]
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string descripcion;

    [Header("Configuración")]
    [SerializeField] private bool destruirAlRecoger = true;
    [SerializeField] private bool recogerSoloUnaVez = true;
    [SerializeField] private AudioClip sonidoRecoger;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject interactionPrompt; // UI "Presiona E"

    private bool yaRecogido = false;
    private bool jugadorCerca = false;

    private void Update()
    {
        // Si el jugador está cerca y presiona E
        if (jugadorCerca && !yaRecogido && Input.GetKeyDown(KeyCode.E))
        {
            RecogerItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaRecogido)
        {
            jugadorCerca = true;
            Debug.Log("Cerca de: " + itemName);

            // Mostrar prompt "Presiona E"
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;

            // Ocultar prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }

    public void RecogerItem()
    {
        // Evitar duplicados si está configurado
        if (recogerSoloUnaVez && yaRecogido)
        {
            Debug.Log($"El item '{itemName}' ya fue recogido.");
            return;
        }

        yaRecogido = true;

        Debug.Log("Recogiendo: " + itemName);

        // Ocultar prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // Opción 1: Usar Singleton (recomendado)
        if (Inventario.Instance != null)
        {
            Inventario.Instance.AddItem(itemName, itemIcon, descripcion);
            Debug.Log($"{itemName} agregado al inventario.");
        }
        else
        {
            // Opción 2: Buscar el objeto (menos eficiente pero funcional)
            GameObject objetoInventario = GameObject.FindWithTag("Inventario");
            if (objetoInventario != null)
            {
                Inventario inventario = objetoInventario.GetComponent<Inventario>();
                if (inventario != null)
                {
                    inventario.AddItem(itemName, itemIcon, descripcion);
                    Debug.Log($"{itemName} agregado al inventario.");
                }
                else
                {
                    Debug.LogWarning("No se encontró el componente Inventario.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el inventario en la escena.");
            }
        }

        // Mostrar mensaje en el juego (si DialogueManager existe)
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble($"Has obtenido: {itemName}", transform);
        }

        // Reproducir sonido si existe
        if (sonidoRecoger != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
        }

        // Destruir el objeto de la escena
        if (destruirAlRecoger)
        {
            Destroy(gameObject);
        }
        else
        {
            // Alternativa: solo desactivarlo
            gameObject.SetActive(false);
        }
    }
}
