using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Clock : MonoBehaviour
{
    [Header("Referencias del reloj")]
    [SerializeField] private Transform hourHand; // manecilla de las horas
    [SerializeField] private Transform minuteHand; // opcional solo decorativa
    [SerializeField] private Button nextHourButton;
    [SerializeField] private Button prevHourButton;
    [SerializeField] private Button confirmButton;

    [Header("Configuración")]
    [SerializeField] private string[] hours = { "9 AM", "12 PM", "3 PM", "6 PM" };
    [SerializeField] private string[] sceneNames = { "Nivel_9AM", "Nivel_12PM", "Nivel_3PM", "Nivel_6PM" };

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
    }

    private void NextHour()
    {
        currentIndex = (currentIndex + 1) % hours.Length;
        UpdateClockRotation();
    }

    private void PreviousHour()
    {
        currentIndex = (currentIndex - 1 + hours.Length) % hours.Length;
        UpdateClockRotation();
    }

    private void UpdateClockRotation()
    {
        float anglePerHour = 360f / hours.Length; // 90° por hora (4 opciones)
        float newRotation = -currentIndex * anglePerHour;

        if (hourHand != null)
            hourHand.localRotation = Quaternion.Euler(0, 0, newRotation);
    }

    private void ConfirmHour()
    {
        if (uiManager != null)
        {
            uiManager.ShowConfirmation(hours[currentIndex], sceneNames[currentIndex]);
        }
        else
        {
            Debug.LogWarning("No se encontró el ClockUIManager en la escena.");
        }
    }
}
