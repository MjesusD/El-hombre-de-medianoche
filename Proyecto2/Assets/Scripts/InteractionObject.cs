using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    private PanelManager panelManager;
    private SpriteRenderer sr;
    private Color originalColor;
    [SerializeField] private Color hoverColor = Color.yellow; 

    void Start()
    {
        panelManager = FindAnyObjectByType<PanelManager>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    void OnMouseEnter()
    {
        if (sr != null)
            sr.color = hoverColor; // cambia de color al entrar el mouse
    }

    void OnMouseExit()
    {
        if (sr != null)
            sr.color = originalColor; // vuelve al color original al salir
    }

    void OnMouseDown()
    {
        if (panelManager != null)
        {
            panelManager.ShowNextPanel();
        }
    }
}
