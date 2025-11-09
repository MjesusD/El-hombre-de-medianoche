using UnityEngine;

public class PickItem : MonoBehaviour
{
    
    public string itemName;
    public Sprite itemIcon;
    public string descripcion;

    [Header("Configuración")]
    [SerializeField] private bool destruirAlRecoger = true;
    [SerializeField] private AudioClip sonidoRecoger;

    private bool yaRecogido = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaRecogido)
        {
            RecogerItem();
        }

        Debug.Log("choque con" + itemName);

        void RecogerItem()
        {
            yaRecogido = true;

            Debug.Log("Recogiendo: " + itemName);

            // Opción 1: Usar Singleton (recomendado)
            if (Inventario.Instance != null)
            {
                Inventario.Instance.AddItem(itemName, itemIcon, descripcion);
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
                    }
                }
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
}
