using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite itemIcon;
    public string description;

    public InventoryItem(string name, Sprite icon, string desc = "")
    {
        itemName = name;
        itemIcon = icon;
        description = desc;
    }
}

public class inventario : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Text itemDescriptionText; //descripcion del item seleccionado

    [Header("Configuración")]
    [SerializeField] private int maxItems = 20;
    [SerializeField] private KeyCode toggleKey = KeyCode.I;
    [SerializeField] private KeyCode useItemKey = KeyCode.E;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    [Header("Navegación con Teclado")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private int itemsPerRow = 5;

    private List<InventoryItem> items = new List<InventoryItem>();
    private List<GameObject> itemSlots = new List<GameObject>();
    private bool isInventoryOpen = false;
    private int selectedIndex = 0;
    private Player player;


    void Start()
    {
        player = FindObjectOfType<Player>();

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Abrir/cerrar inventario
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }

        // Si el inventario está abierto, manejar navegación
        if (isInventoryOpen)
        {
            HandleInventoryNavigation();

            // Cerrar con ESC
            if (Input.GetKeyDown(closeKey))
            {
                ToggleInventory();
            }

            // Usar item con E
            if (Input.GetKeyDown(useItemKey) && items.Count > 0)
            {
                UseSelectedItem();
            }
        }
    }

    void HandleInventoryNavigation()
    {
        if (items.Count == 0) return;

        int oldIndex = selectedIndex;

        // Navegación arriba
        if (Input.GetKeyDown(upKey))
        {
            selectedIndex -= itemsPerRow;
            if (selectedIndex < 0)
                selectedIndex = 0;
        }

        // Navegación abajo
        if (Input.GetKeyDown(downKey))
        {
            selectedIndex += itemsPerRow;
            if (selectedIndex >= items.Count)
                selectedIndex = items.Count - 1;
        }

        // Navegación izquierda
        if (Input.GetKeyDown(leftKey))
        {
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = 0;
        }

        // Navegación derecha
        if (Input.GetKeyDown(rightKey))
        {
            selectedIndex++;
            if (selectedIndex >= items.Count)
                selectedIndex = items.Count - 1;
        }

        // Si cambió la selección, actualizar visual
        if (oldIndex != selectedIndex)
        {
            UpdateSelection();
        }
    }

    public void AddItem(string itemName, Sprite itemIcon, string description = "")
    {
        if (items.Count >= maxItems)
        {
            Debug.Log("¡Inventario lleno!");
            return;
        }

        InventoryItem newItem = new InventoryItem(itemName, itemIcon, description);
        items.Add(newItem);

        Debug.Log("Item agregado: " + itemName);

        // Si el inventario está abierto, actualizar UI
        if (isInventoryOpen)
        {
            UpdateInventoryUI();
        }
    }

    public void RemoveItem(string itemName)
    {
        InventoryItem itemToRemove = items.Find(item => item.itemName == itemName);

        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
            Debug.Log("Item removido: " + itemName);

            // Ajustar índice seleccionado si es necesario
            if (selectedIndex >= items.Count)
            {
                selectedIndex = Mathf.Max(0, items.Count - 1);
            }

            UpdateInventoryUI();
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isInventoryOpen);
        }

        // Pausar/despausar al jugador
        if (player != null)
        {
            player.SetCanMove(!isInventoryOpen);
        }

        if (isInventoryOpen)
        {
            selectedIndex = 0;
            UpdateInventoryUI();
        }
    }

    void UpdateInventoryUI()
    {
        if (itemsContainer == null) return;

        // Limpiar slots existentes
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }
        itemSlots.Clear();

        // Crear slots para cada item
        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];
            GameObject slot = Instantiate(itemSlotPrefab, itemsContainer);
            itemSlots.Add(slot);

            // Configurar icono
            Image iconImage = slot.transform.Find("Icon")?.GetComponent<Image>();
            if (iconImage != null && item.itemIcon != null)
            {
                iconImage.sprite = item.itemIcon;
                iconImage.enabled = true;
            }

            // Configurar nombre (opcional)
            Text itemText = slot.transform.Find("ItemName")?.GetComponent<Text>();
            if (itemText != null)
            {
                itemText.text = item.itemName;
            }
        }

        UpdateSelection();
    }

    void UpdateSelection()
    {
        // Actualizar highlight visual de todos los slots
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Image background = itemSlots[i].GetComponent<Image>();
            if (background != null)
            {
                background.color = (i == selectedIndex) ? Color.yellow : Color.white;
            }
        }

        // Actualizar descripción del item seleccionado
        if (itemDescriptionText != null && items.Count > 0 && selectedIndex < items.Count)
        {
            itemDescriptionText.text = items[selectedIndex].description;
        }
    }

    void UseSelectedItem()
    {
        if (selectedIndex >= items.Count) return;

        InventoryItem selectedItem = items[selectedIndex];
        Debug.Log("Usando item: " + selectedItem.itemName);

        // Aquí puedes agregar lógica específica para usar items
        // Por ejemplo: si es una llave, abrir una puerta
        // Si es medicina, curar al jugador, etc.
    }

    public List<InventoryItem> GetAllItems()
    {
        return items;
    }
}
