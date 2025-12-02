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

    // ID del punto de aparición cuando se viaja con el reloj
    [SerializeField] private string clockSpawnID = "ClockSpawn";

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
            // Guardar el punto de spawn
            SpawnManager.nextSpawnID = clockSpawnID;

            // Obtener referencia al jugador actual
            var spawnManager = FindAnyObjectByType<SpawnManager>();
            var player = spawnManager != null ? spawnManager.GetPlayer() : null;

            if (player != null)
            {
                var playerScript = player.GetComponent<Player>();
                if (playerScript != null)
                    playerScript.SetCanMove(true);
            }

            // Asegurar que el tiempo corra normalmente
            Time.timeScale = 1f;

            // Cargar la nueva escena
            SceneManager.LoadScene(targetScene);
        }
    }

    private void OnNo()
    {
        confirmationPanel.SetActive(false);
    }
}
