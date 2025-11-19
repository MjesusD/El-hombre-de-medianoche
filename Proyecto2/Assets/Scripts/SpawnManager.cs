using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static string nextSpawnID;
    [SerializeField] private GameObject playerPrefab;

    private GameObject currentPlayer;

    private void Awake()
    {
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

        // No crear jugador en el menú principal
        if (scene.name == "MainMenuScene")
        {
            if (currentPlayer != null)
                Destroy(currentPlayer);

            currentPlayer = null;
            return;
        }

        Vector3 spawnPosition = Vector3.zero;
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

        if (currentPlayer != null)
        {
            currentPlayer.transform.position = spawnPosition;
        }
        else
        {
            currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            DontDestroyOnLoad(currentPlayer);
        }

        CinemachineCamera vcam = FindAnyObjectByType<CinemachineCamera>();
        if (vcam != null)
            vcam.Follow = currentPlayer.transform;

        var playerScript = currentPlayer.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.enabled = true;
            playerScript.SetCanMove(true);
        }

        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }

    public GameObject GetPlayer()
    {
        return currentPlayer;
    }
}
