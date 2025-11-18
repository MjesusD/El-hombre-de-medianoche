using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "Barco"; // escena de juego real
    [SerializeField] private IntroSystem introSystem;         // referencia al script de la intro
    [SerializeField] private GameObject controlsPanel;        // panel de controles

    public void PlayGame()
    {
        if (PlayerPrefs.GetInt("IntroPlayed", 0) == 1)
        {
            SceneManager.LoadScene(firstSceneName);
            return;
        }

        if (introSystem != null)
        {
            introSystem.StartIntro();
        }
        else
        {
            Debug.LogError("No se asignó IntroSystem en GameManager");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego...");
        Application.Quit();
    }

    public void ResetIntro()
    {
        PlayerPrefs.DeleteKey("IntroPlayed");
        Debug.Log("Intro reseteada");
    }

    // Abrir el panel de controles
    public void ShowControls()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(true);
        else
            Debug.LogWarning("No se asignó el panel de controles");
    }

    // Cerrar el panel de controles
    public void HideControls()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(false);
    }
}
