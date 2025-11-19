using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Panel de Pausa")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Configuración")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);

            Time.timeScale = 1f;
            isPaused = false;
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("No se asignó el panel del menú de pausa.");
            return;
        }

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (Player.Instance != null)
            Player.Instance.SetCanMove(false);
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        if (Player.Instance != null)
            Player.Instance.SetCanMove(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
