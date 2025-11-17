using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [Header("Configuración del Reloj")]
    [SerializeField] private string relojNombre = "Reloj";
    [SerializeField] private Sprite relojIcono;
    [TextArea][SerializeField] private string descripcion = "Permite viajar en el tiempo.";
    [SerializeField] private bool entregarSoloUnaVez = true;

    private bool entregado = false;
    private Inventario inventario;

    private void Start()
    {
        inventario = FindAnyObjectByType<Inventario>();
    }

    public void EntregarReloj()
    {
        // Evita duplicados
        if (entregarSoloUnaVez && entregado)
        {
            Debug.Log("El reloj ya fue entregado.");
            return;
        }

        if (inventario == null)
        {
            Debug.LogWarning("No se encontró el inventario en la escena.");
            return;
        }

        // Agregar al inventario
        inventario.AddItem(relojNombre, relojIcono, descripcion);
        entregado = true;

        Debug.Log("Has obtenido el reloj.");

        // Mostrar mensaje
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble("Has obtenido el reloj.", transform);
        }

        // Destruir el reloj en la escena después de recogerlo
        Destroy(gameObject);
    }
}
