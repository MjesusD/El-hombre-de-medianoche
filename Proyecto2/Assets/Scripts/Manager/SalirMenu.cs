using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenuInteract : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("Interacción, Saliendo al menú…");
        SceneManager.LoadScene("MainMenuScene");
    }
}
