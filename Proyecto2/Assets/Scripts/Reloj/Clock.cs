using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    [Header("Referencias del reloj")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Button nextHourButton;
    [SerializeField] private Button prevHourButton;
    [SerializeField] private Button confirmButton;

    [Header("Configuración")]
    [SerializeField] private string[] hours = { "9 AM", "12 PM", "3 PM", "6 PM" };
    [SerializeField] private string[] sceneNames = { "Nivel_9AM", "Nivel_12PM", "Nivel_3PM", "Nivel_6PM" };

    [Header("Progreso del jugador")]
    [Tooltip("Marca cuáles horas están desbloqueadas")]
    [SerializeField] private bool[] unlockedHours = { true, false, false, false };

    private int currentIndex = 0;
    private Clock_UIManager uiManager;

    private void Start()
    {
        uiManager = FindAnyObjectByType<Clock_UIManager>();

        if (nextHourButton != null)
            nextHourButton.onClick.AddListener(NextHour);
        if (prevHourButton != null)
            prevHourButton.onClick.AddListener(PreviousHour);
        if (confirmButton != null)
            confirmButton.onClick.AddListener(ConfirmHour);

        UpdateClockRotation();
        UpdateConfirmButtonState();
    }

    private void NextHour()
    {
        currentIndex = (currentIndex + 1) % hours.Length;
        UpdateClockRotation();
        UpdateConfirmButtonState();
    }

    private void PreviousHour()
    {
        currentIndex = (currentIndex - 1 + hours.Length) % hours.Length;
        UpdateClockRotation();
        UpdateConfirmButtonState();
    }

    private void UpdateClockRotation()
    {
        float anglePerHour = 360f / hours.Length;
        float newRotation = -currentIndex * anglePerHour;

        if (hourHand != null)
            hourHand.localRotation = Quaternion.Euler(0, 0, newRotation);
    }

    private void UpdateConfirmButtonState()
    {
        if (confirmButton != null)
        {
            confirmButton.interactable = unlockedHours[currentIndex];
        }
    }

    private void ConfirmHour()
    {
        if (!unlockedHours[currentIndex])
        {
            Debug.Log("Esta hora aún no está desbloqueada.");
            return;
        }

        if (uiManager != null)
        {
            uiManager.ShowConfirmation(hours[currentIndex], sceneNames[currentIndex]);
        }
        else
        {
            Debug.LogWarning("No se encontró el ClockUIManager en la escena.");
        }
    }

    // Método opcional para desbloquear horas desde otros scripts
    public void UnlockHour(int index)
    {
        if (index >= 0 && index < unlockedHours.Length)
            unlockedHours[index] = true;
    }
}
