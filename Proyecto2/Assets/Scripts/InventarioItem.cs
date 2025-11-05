using UnityEngine;
[System.Serializable]
public class InventarioItem : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public string descripcion;

    public InventarioItem(string name, Sprite icon, string desc = "")
    {
        itemName = name;
        itemIcon = icon;
        descripcion = desc;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("choque con" + itemName);
        //Inventario.Instance.AddItem(itemName, itemIcon, descripcion);
        GameObject miObjeto = GameObject.FindWithTag("Inventario");
        Inventario inventario = miObjeto .GetComponent<Inventario>();
        inventario.AddItem(itemName, itemIcon, descripcion);
        Destroy(gameObject);
    }

}
