using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

public class inventario : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject inventarioPanel;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescripcionText; //descripcion del item seleccionado

    [Header("Configuracion")]
    [SerializeField] private int maxItems = 20;
    [SerializeField] private KeyCode toggleKey = KeyCode.I;
    [SerializeField] private KeyCode useItemKey = KeyCode.E;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    [Header("Navegacion")]
    [SerializeField] private int itemsPerRow = 5;

    private List<InventarioItem> items = new List<InventarioItem>();
    private List<GameObject> itemSlots = new List<GameObject>();
    private bool isInventoryOpen = false;
    private int selectedIndex = 0;
    private Player player;
    private AudioSource audioSource;
    public static inventario Instance { get; private set; }

    void Awake()
    {
        // Configurar singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = FindAnyObjectByType<Player>();

        if (inventarioPanel != null)
        {
            inventarioPanel.SetActive(false);
        }
        if(itemsContainer != null)
        {
            GridLayoutGroup grid = itemsContainer.GetComponent<GridLayoutGroup>();
            if (grid == null)
            {
                grid = itemsContainer.AddComponent<GridLayoutGroup>();
            }
            grid.cellSize = new Vector2(80, 80);
            grid.spacing = new Vector2(10, 10);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = itemsPerRow;
        }
    }

    void Update()
    {
        // Abrir/cerrar inventario
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }

        // Si el inventario est� abierto, manejar navegaci�n
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

        // Navegaci�n con flechas o WASD
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex -= itemsPerRow;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex += itemsPerRow;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex--;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex++;
        }

        // Limitar �ndice
        selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Count - 1);

        if (oldIndex != selectedIndex)
        {
            UpdateSelection();
        }
    }


    public void AddItem(string itemName, Sprite itemIcon, string description = "")
    {
        if (items.Count >= maxItems)
        {
            Debug.Log("�Inventario lleno!");
            return;
        }

        InventarioItem newItem = new InventarioItem(itemName, itemIcon, description);
        items.Add(newItem);

        Debug.Log("Item agregado: " + itemName);

        // Si el inventario est� abierto, actualizar UI
        if (isInventoryOpen)
        {
            UpdateInventoryUI();
        }
    }

    public void RemoveItem(string itemName)
    {
        InventarioItem itemToRemove = items.Find(item => item.itemName == itemName);

        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
            Debug.Log("Item removido: " + itemName);

            // Ajustar �ndice seleccionado si es necesario
            if (selectedIndex >= items.Count && items.Count > 0)
            {
                selectedIndex = items.Count - 1;
            }
            if (isInventoryOpen)
            {
                UpdateInventoryUI();
            }          
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventarioPanel != null)
        {
            inventarioPanel.SetActive(isInventoryOpen);
        }

        // Pausar/despausar al jugador
        if (player != null)
        {
            player.SetCanMove(!isInventoryOpen);
        }

        if (isInventoryOpen)
        {
            if (items.Count > 0)
            {
                selectedIndex = 0;
            }
            UpdateInventoryUI();
        }
    }

    void UpdateInventoryUI()
    {
        if (itemsContainer == null) return;

        //limpiar slots 
        foreach (Transform child in itemsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        itemSlots.Clear();

        //crear slots para cada item
        for (int i = 0; i < items.Count; i++)
        {
            InventarioItem item = items[i];
            GameObject slot = Instantiate(itemSlotPrefab, itemsContainer.transform);
            itemSlots.Add(slot);

            // Configurar slot
            Image slotBg = slot.GetComponent<Image>();

            // Buscar componentes del slot
            Image iconImage = slot.transform.Find("Icon")?.GetComponent<Image>();
            if (iconImage != null)
            {
                if (item.itemIcon != null)
                {
                    iconImage.sprite = item.itemIcon;
                    iconImage.color = Color.white;
                }
                else
                {
                    iconImage.color = new Color(1, 1, 1, 0.3f);
                }
            }
        }

        UpdateSelection();
    }

    void UpdateSelection()
    {
        // Actualizar highlight visual de todos los slots
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Image bg = itemSlots[i].GetComponent<Image>();
            if (bg != null)
            {
                bg.color = (i == selectedIndex) ? new Color(1f, 1f, 0.3f, 1f) : new Color(0.2f, 0.2f, 0.2f, 0.8f);
            }

            // Agregar borde al seleccionado
            Outline outline = itemSlots[i].GetComponent<Outline>();
            if (outline == null && i == selectedIndex)
            {
                outline = itemSlots[i].AddComponent<Outline>();
                outline.effectColor = Color.yellow;
                outline.effectDistance = new Vector2(3, 3);
            }
            else if (outline != null && i != selectedIndex)
            {
                Destroy(outline);
            }
        }

        // Actualizar descripci�n del item seleccionado
        if (items.Count > 0 && selectedIndex < items.Count)
        {
            InventarioItem selected = items[selectedIndex];

            if (itemNameText != null)
            {
                itemNameText.text = selected.itemName;
            }

            if (itemDescripcionText != null)
            {
                itemDescripcionText.text = selected.descripcion;
            }
        }
        else
        {
            if (itemNameText != null) itemNameText.text = "";
            if (itemDescripcionText != null) itemDescripcionText.text = "Inventario vac�o";
        }
    }

    void UseSelectedItem()
    {
        if (selectedIndex >= items.Count) return;

        InventarioItem selectedItem = items[selectedIndex];
        Debug.Log("Usando: " + selectedItem.itemName);

        // Aqui agregar logica especifica por item
        // Ejemplo:
        // if (selectedItem.itemName == "Llave") { AbrirPuerta(); }
    }

    public List<InventarioItem> GetAllItems()
    {
        return items;
    }
} 
