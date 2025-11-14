using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "Barco"; // escena de juego real
    [SerializeField] private IntroSystem introSystem;         // referencia al script de la intro

    public void PlayGame()
    {
        // Se mostró la intro antes?
        if (PlayerPrefs.GetInt("IntroPlayed", 0) == 1)
        {
            // Saltar directo al juego
            SceneManager.LoadScene(firstSceneName);
            return;
        }

        // Primera vez ejecutar intro
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

    // para testear la intro de nuevo
    public void ResetIntro()
    {
        PlayerPrefs.DeleteKey("IntroPlayed");
        Debug.Log("Intro reseteada");
    }
}
