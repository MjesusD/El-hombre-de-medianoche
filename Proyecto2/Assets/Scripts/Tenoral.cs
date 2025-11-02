using UnityEngine;

public class PruebaInventario : MonoBehaviour
{
    [Header("Item de Prueba")]
    [SerializeField] private Sprite iconoPrueba;

    void Update()
    {
        // Presiona T para agregar item
        if (Input.GetKeyDown(KeyCode.T))
        {
            inventario.Instance.AddItem(
                "Llave Misteriosa",
                iconoPrueba,
                "Una llave antigua que parece abrir algo importante."
            );
        }

        // Presiona R para remover item
        if (Input.GetKeyDown(KeyCode.R))
        {
            inventario.Instance.RemoveItem("Llave Misteriosa");
        }

        // Presiona Y para agregar otro item
        if (Input.GetKeyDown(KeyCode.Y))
        {
            inventario.Instance.AddItem(
                "Nota Vieja",
                null,
                "Un papel arrugado con escritura ilegible."
            );
        }

        // Presiona U para agregar más items de prueba
        if (Input.GetKeyDown(KeyCode.U))
        {
            inventario.Instance.AddItem(
                "Moneda Dorada",
                iconoPrueba,
                "Una moneda brillante de origen desconocido."
            );
        }

        // Presiona P para verificar si tiene un item
        if (Input.GetKeyDown(KeyCode.P))
        {
            bool tieneLlave = inventario.Instance.HasItem("Llave Misteriosa");
            Debug.Log("¿Tiene Llave Misteriosa? " + (tieneLlave ? "SÍ" : "NO"));
        }
    }
}