using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Escena de juego principal")]
    [SerializeField] private string firstSceneName = "Barco";

    [Header("Intro")]
    [SerializeField] private IntroSystem introSystem;
    private bool introRunning = false;

    [Header("Panel de Controles")]
    [SerializeField] private GameObject controlsPanel;

    public void PlayGame()
    {
        // Evitar que se presione Play dos veces
        if (introRunning)
            return;

        introRunning = true;

        // Siempre reproducir la intro
        if (introSystem != null)
        {
            introSystem.StartIntro();
        }
        else
        {
            Debug.LogError("No se asignó IntroSystem en GameManager. No se puede iniciar la intro.");
            // Si falta IntroSystem, cargar juego igual para evitar que se trabe
            SceneManager.LoadScene(firstSceneName);
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(firstSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego...");
        Application.Quit();
    }

    public void ShowControls()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(true);
        else
            Debug.LogWarning("No se asignó el panel de controles");
    }

    public void HideControls()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(false);
    }
}
