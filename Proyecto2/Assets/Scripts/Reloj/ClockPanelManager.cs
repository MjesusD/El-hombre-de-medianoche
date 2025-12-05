using UnityEngine;

public class ClockPanelManager : MonoBehaviour
{
    public static ClockPanelManager Instance;

    [Header("Panel del reloj")]
    [SerializeField] private GameObject clockPanel; // el Canvas del reloj completo

    private bool isOpen = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (clockPanel != null)
            clockPanel.SetActive(false);
    }

    public void ShowClockPanel()
    {
        if (clockPanel == null) return;

        clockPanel.SetActive(true);
        isOpen = true;

        // Pausar juego o bloquear movimiento del jugador
        Time.timeScale = 0f;
    }

    public void HideClockPanel()
    {
        if (clockPanel == null) return;

        clockPanel.SetActive(false);
        isOpen = false;

        // Reanudar juego
        Time.timeScale = 1f;
    }

    public bool IsOpen() => isOpen;
}
