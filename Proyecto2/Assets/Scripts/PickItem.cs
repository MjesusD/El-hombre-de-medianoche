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

    [Header("Opciones especiales")]
    public bool noAdd = false;   // PARA QUE NO SE GUARDEN EN EL INVENTARIO

    [Header("Visual Feedback")]
    [SerializeField] private GameObject interactionPrompt; // UI "Presiona E"

    private bool yaRecogido = false;
    private bool jugadorCerca = false;

    private void Update()
    {
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

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }

    public void RecogerItem()
    {
        if (recogerSoloUnaVez && yaRecogido)
        {
            Debug.Log($"El item '{itemName}' ya fue recogido.");
            return;
        }

        yaRecogido = true;

        Debug.Log("Recogiendo: " + itemName);

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // No agregar los marcados con noAdd
        if (!noAdd)
        {
            
            if (Inventario.Instance != null)
            {
                Inventario.Instance.AddItem(itemName, itemIcon, descripcion);
                Debug.Log($"{itemName} agregado al inventario.");
            }
            else
            {
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
        }
        else
        {
            Debug.Log($"{itemName} es noAdd: NO se agrega al inventario.");
        }

        // Feedback visual existente
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble($"Has obtenido: {itemName}", transform);
        }

        if (sonidoRecoger != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
        }

        if (destruirAlRecoger)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
