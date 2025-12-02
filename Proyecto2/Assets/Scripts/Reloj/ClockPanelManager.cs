using UnityEngine;

public class ClockPanelManager : MonoBehaviour
{
    public static ClockPanelManager Instance;

    [Header("Panel del reloj")]
    [SerializeField] private GameObject clockCanvas; // el Canvas del reloj completo

    private bool isOpen = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (clockCanvas != null)
            clockCanvas.SetActive(false);
    }

    public void ShowClockPanel()
    {
        if (clockCanvas == null) return;

        clockCanvas.SetActive(true);
        isOpen = true;

        // Pausar juego o bloquear movimiento del jugador
        Time.timeScale = 0f;
    }

    public void HideClockPanel()
    {
        if (clockCanvas == null) return;

        clockCanvas.SetActive(false);
        isOpen = false;

        // Reanudar juego
        Time.timeScale = 1f;
    }

    public bool IsOpen() => isOpen;
}
