using UnityEngine;

public class ObjetoPista : MonoBehaviour
{
    [Header("Información de la Pista")]
    [SerializeField] private string nombrePista = "Primer Dígito";
    [SerializeField] private string mensajePista = "Encontraste un número escrito en la pared: 5";
    [SerializeField] private Sprite iconoPista; //icono para el inventario

    [Header("Configuración")]
    [SerializeField] private bool destruirDespuesDeUsar = false;
    [SerializeField] private AudioClip sonidoEncontrar;

    [Header("Visual")]
    [SerializeField] private GameObject efectoParticulas; //efecto visual al encontrar

    private bool yaEncontrada = false;
    private InteractionObject interactionObject;

    private void Start()
    {
        interactionObject = GetComponent<InteractionObject>();
    }

    //metodo llamado desde InteractionObject cuando el jugador interactua
    public void RevelarPista()
    {
        if (yaEncontrada)
        {
            Debug.Log("Ya encontraste esta pista.");

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.ShowBubble("Ya examinaste esto.", transform);
            }
            return;
        }

        yaEncontrada = true;

        //registrar la pista en el sistema
        if (SistemaPistas.Instancia != null)
        {
            SistemaPistas.Instancia.EncontrarPista(nombrePista);
        }

        //mostrar mensaje
        Debug.Log($"¡Pista encontrada! {mensajePista}");

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble(mensajePista, transform);
        }

        // Reproducir sonido
        if (sonidoEncontrar != null)
        {
            AudioSource.PlayClipAtPoint(sonidoEncontrar, transform.position);
        }

        // Efecto de partículas
        if (efectoParticulas != null)
        {
            Instantiate(efectoParticulas, transform.position, Quaternion.identity);
        }

        // Destruir o desactivar el objeto
        if (destruirDespuesDeUsar)
        {
            Invoke("DestruirObjeto", 1f);
        }
        else
        {
            //cambiar el mensaje de interaccion
            if (interactionObject != null)
            {
                //desactivar para que no se pueda volver a examinar
                interactionObject.enabled = false;
            }
        }
    }

    private void DestruirObjeto()
    {
        Destroy(gameObject);
    }
}
