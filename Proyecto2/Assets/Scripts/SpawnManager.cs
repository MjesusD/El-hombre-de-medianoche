using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static string nextSpawnID; // se asigna antes de cambiar de escena
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        // Asegura que el SpawnManager exista en todas las escenas
        if (FindObjectsByType<SpawnManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
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
        if (playerPrefab == null)
        {
            Debug.LogError("No se ha asignado el prefab del jugador en SpawnManager.");
            return;
        }

        GameObject playerInstance = null;
        Vector3 spawnPosition = Vector3.zero;

        // Buscar spawn correcto si hay un ID guardado
        SpawnPoint[] spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        if (!string.IsNullOrEmpty(nextSpawnID))
        {
            foreach (var spawn in spawns)
            {
                if (spawn.GetID() == nextSpawnID)
                {
                    spawnPosition = spawn.transform.position;
                    nextSpawnID = null;
                    break;
                }
            }
        }
        else
        {
            // Si no hay spawn definido, usar el primer SpawnPoint disponible
            if (spawns.Length > 0)
            {
                spawnPosition = spawns[0].transform.position;
            }
            else
            {
                Debug.LogWarning("No se encontró un SpawnPoint en la escena. Se usará (0,0).");
            }
        }

        // Instanciar el jugador en la posición calculada
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        // Reasignar seguimiento de cámara Cinemachine
        CinemachineCamera vcam = FindAnyObjectByType<CinemachineCamera>();
        if (vcam != null && playerInstance != null)
        {
            vcam.Follow = playerInstance.transform;
        }
        else if (vcam == null)
        {
            Debug.LogWarning("No se encontró una CinemachineCamera en la escena.");
        }
    }
}
