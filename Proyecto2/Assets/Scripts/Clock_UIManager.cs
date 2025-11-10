using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Clock_UIManager : MonoBehaviour
{
    [Header("UI Confirmación")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private string targetScene;

    private void Start()
    {
        confirmationPanel.SetActive(false);

        yesButton.onClick.AddListener(OnYes);
        noButton.onClick.AddListener(OnNo);
    }

    public void ShowConfirmation(string hourText, string sceneName)
    {
        targetScene = sceneName;
        confirmationPanel.SetActive(true);
        questionText.text = $"¿Ir a las {hourText}?";
    }

    private void OnYes()
    {
        confirmationPanel.SetActive(false);

        if (!string.IsNullOrEmpty(targetScene))
        {
            // Aquí se puede guardar el spawn ID antes de cambiar
            // SpawnManager.nextSpawnID = "SpawnDesdeReloj";
            SceneManager.LoadScene(targetScene);
        }
    }

    private void OnNo()
    {
        confirmationPanel.SetActive(false);
    }
}
