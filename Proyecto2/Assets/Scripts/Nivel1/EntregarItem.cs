using UnityEngine;

public class EntregaItem : MonoBehaviour
{
    [Header("Item a Entregar")]
    [SerializeField] private string nombreItem = "Llave Maestra";
    [SerializeField] private Sprite iconoItem;
    [TextArea(2, 5)]
    [SerializeField] private string descripcionItem = "Una llave importante.";

    [Header("Configuración")]
    [SerializeField] private bool entregarAlFinalDelDialogo = true; // Entrega automáticamente al terminar diálogo
    [SerializeField] private bool soloEntregarUnaVez = true;
    [SerializeField] private string tagInkParaEntregar = "entregarItem"; // Tag en Ink que activa la entrega

    [Header("Condiciones (Opcional)")]
    [SerializeField] private bool requiereCondicion = false;
    [SerializeField] private string itemRequerido = ""; // Item que debe tener para recibir este

    private bool itemEntregado = false;

    private InkTrigger inkTrigger;

    void Start()
    {
        inkTrigger = GetComponent<InkTrigger>();

        // Suscribirse a los tags de Ink
        InkDialogueManager.OnTagsReceived += OnInkTag;
    }

    void OnDestroy()
    {
        // Desuscribirse cuando se destruya
        InkDialogueManager.OnTagsReceived -= OnInkTag;
    }

    // Escucha los tags que vienen desde Ink
    void OnInkTag(System.Collections.Generic.List<string> tags)
    {
        if (tags == null) return;

        foreach (string tag in tags)
        {
            // Si el diálogo de Ink tiene el tag "entregarItem"
            if (tag == tagInkParaEntregar)
            {
                EntregarItem();
            }
        }
    }

    // Método llamado manualmente si no usas Ink
    public void EntregarItemManual()
    {
        EntregarItem();
    }

    void EntregarItem()
    {
        if (itemEntregado && soloEntregarUnaVez)
        {
            Debug.Log("Este NPC ya entregó su item.");
            return;
        }

        // Verificar condición si está activa
        if (requiereCondicion && !string.IsNullOrEmpty(itemRequerido))
        {
            if (Inventario.Instance == null || !Inventario.Instance.HasItem(itemRequerido))
            {
                MostrarMensaje($"Primero necesitas conseguir: {itemRequerido}");
                return;
            }
        }

        // Verificar que el inventario exista
        if (Inventario.Instance == null)
        {
            Debug.LogError("No se encontró el inventario!");
            return;
        }

        // Agregar item al inventario
        Inventario.Instance.AddItem(nombreItem, iconoItem, descripcionItem);

        itemEntregado = true;

        // Mostrar mensaje de éxito
        MostrarMensaje($"Recibiste: {nombreItem}");

        Debug.Log($"✓ {nombreItem} entregado al jugador por {gameObject.name}");
    }

    void MostrarMensaje(string mensaje)
    {
        // Usar DialogueManager si existe
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble(mensaje, transform);
        }
        else
        {
            Debug.Log($"{gameObject.name}: {mensaje}");
        }
    }

    // Método público para verificar si ya entregó el item
    public bool ItemYaEntregado()
    {
        return itemEntregado;
    }

    // Método para resetear (útil para testing)
    public void Resetear()
    {
        itemEntregado = false;
    }

    // Método para forzar entrega (para eventos especiales)
    public void ForzarEntrega()
    {
        EntregarItem();
    }
}
