using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Método que se llama desde el botón
    public void PlayGame()
    {
        SceneManager.LoadScene("Principal_P1");
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego...");
        Application.Quit();
    }
}
