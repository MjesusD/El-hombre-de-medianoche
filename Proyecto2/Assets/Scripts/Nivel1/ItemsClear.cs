using UnityEngine;

public class ItemsClear : MonoBehaviour
{
    private string nombreItem = "Reloj";
    [SerializeField] private Sprite iconoItem;
    private string descripcionItem = "Reloj antiguo. Permite viajar en el tiempo.";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Inventario.Instance.items.Clear();
        Inventario.Instance.AddItem(nombreItem, iconoItem, descripcionItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
