using UnityEngine;

public class PickItem : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public string descripcion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("choque con" + itemName);
        //Inventario.Instance.AddItem(itemName, itemIcon, descripcion);
        GameObject miObjeto = GameObject.FindWithTag("Inventario");
        Inventario inventario = miObjeto.GetComponent<Inventario>();
        inventario.AddItem(itemName, itemIcon, descripcion);

    }
}
