using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : InteractionObject
{
    [SerializeField] private string sceneToLoad;

    public override void Interact()
    {
        // Aquí haces el cambio de escena
        SceneManager.LoadScene(sceneToLoad);
    }
}
