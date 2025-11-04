using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnID;

    public string GetID()
    {
        return spawnID;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, spawnID);
    }
#endif
}
