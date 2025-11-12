using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("Paneles registrados")]
    [Tooltip("Lista de paneles que se pueden mostrar por nombre o índice.")]
    public GameObject[] panels;

    [Header("Dependencias")]
    [SerializeField] private CameraSwitcher cameraSwitcher;

    private int currentPanel = -1;

    void Start()
    {
        // Desactiva todos al inicio
        foreach (var p in panels)
            if (p != null)
                p.SetActive(false);
    }

    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length) return;

        HideAllPanels();
        panels[index].SetActive(true);
        currentPanel = index;

        if (cameraSwitcher != null && !cameraSwitcher.IsInUICamera())
            cameraSwitcher.SwitchToUICamera();
    }

    public void ShowPanel(string panelName)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null && panels[i].name == panelName)
            {
                ShowPanel(i);
                return;
            }
        }
        Debug.LogWarning("Panel no encontrado: " + panelName);
    }

    public void HideAllPanels()
    {
        foreach (var p in panels)
            if (p != null)
                p.SetActive(false);

        currentPanel = -1;

        if (cameraSwitcher != null && cameraSwitcher.IsInUICamera())
            cameraSwitcher.SwitchToMainCamera();
    }

    public void ShowNextPanel()
    {
        if (panels.Length == 0) return;

        int next = (currentPanel + 1) % panels.Length;
        ShowPanel(next);
    }
}
