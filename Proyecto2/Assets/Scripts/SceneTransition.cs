using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Configuración de transición")]
    [SerializeField] private string sceneToLoad; // nombre de la escena destino
    [SerializeField] private string destinationSpawnID; // ID del punto donde aparecerá el jugador

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SpawnManager.nextSpawnID = destinationSpawnID;
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ninguna escena para cargar en SceneTransition.");
        }
    }
}
