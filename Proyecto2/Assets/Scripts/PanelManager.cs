using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("Paneles en orden")]
    public GameObject[] panels;
    private int currentPanel = 0;

    void Start()
    {
        // Ocultar todos menos el primero
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }
    }

    public void ShowNextPanel()
    {
        if (currentPanel < panels.Length - 1)
        {
            panels[currentPanel].SetActive(false);
            currentPanel++;
            panels[currentPanel].SetActive(true);
        }
        else
        {
            // Todos los paneles mostrados
            Debug.Log("Fin de la secuencia de paneles");
        }
    }
}
