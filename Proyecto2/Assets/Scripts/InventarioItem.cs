using UnityEngine;
[System.Serializable]
public class InventarioItem
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
}
