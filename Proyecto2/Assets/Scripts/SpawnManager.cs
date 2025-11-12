using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static string nextSpawnID; // Se asigna antes de cambiar de escena
    [SerializeField] private GameObject playerPrefab;

    private GameObject currentPlayer; // Referencia persistente al jugador

    private void Awake()
    {
        // Asegurar instancia única
        var managers = FindObjectsByType<SpawnManager>(FindObjectsSortMode.None);
        if (managers.Length > 1)
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

        Vector3 spawnPosition = Vector3.zero;

        // Buscar spawn correcto
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
        else if (spawns.Length > 0)
        {
            spawnPosition = spawns[0].transform.position;
        }
        else
        {
            Debug.LogWarning("No se encontró un SpawnPoint en la escena. Se usará (0,0,0).");
        }

        // Si ya hay un jugador persistente, moverlo
        if (currentPlayer != null)
        {
            currentPlayer.transform.position = spawnPosition;
        }
        else
        {
            // Crear el jugador por primera vez
            currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            DontDestroyOnLoad(currentPlayer);
        }

        // Reasignar la cámara Cinemachine
        CinemachineCamera vcam = FindAnyObjectByType<CinemachineCamera>();
        if (vcam != null)
        {
            vcam.Follow = currentPlayer.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró una CinemachineVirtualCamera en la escena.");
        }

        // Reactivar movimiento del jugador al llegar a la nueva escena
        var playerScript = currentPlayer.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.enabled = true;
            playerScript.SetCanMove(true);
        }

        Debug.Log($"[SpawnManager] Jugador actual: {currentPlayer.name}, Activo: {currentPlayer.activeSelf}, Script Player: {currentPlayer.GetComponent<Player>().enabled}");
        
        if (Time.timeScale == 0f)
        {
            Debug.LogWarning("[SpawnManager] Corrigiendo Time.timeScale = 0");
            Time.timeScale = 1f;
        }

    }



    public GameObject GetPlayer()
    {
        return currentPlayer;
    }
}
