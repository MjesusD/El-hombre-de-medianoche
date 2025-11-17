using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    /*[Header("Objeto a generar al terminar el diálogo")]
    public GameObject itemPrefab; // Prefab del reloj u otro objeto

    [Header("Punto donde aparecerá el objeto")]
    public Transform spawnPoint;

    private bool spawned = false;
    private DialogueTrigger myTrigger;

    void Start()
    {
        myTrigger = GetComponent<DialogueTrigger>();

        // Escuchar el evento de diálogo terminado
        DialoguePlayerSystem.OnDialogueFinished += OnDialogueFinished;
    }

    void OnDestroy()
    {
        DialoguePlayerSystem.OnDialogueFinished -= OnDialogueFinished;
    }

    void OnDialogueFinished(DialogueData finalizado)
    {
        // Este NPC solo responde a SU propio diálogo
        if (myTrigger == null) return;
        if (finalizado != myTrigger.dialogue) return;

        // Evitar doble spawn
        if (spawned) return;

        // Instanciar el objeto
        if (itemPrefab != null && spawnPoint != null)
        {
            Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
            spawned = true;

            Debug.Log("Objeto generado después de hablar con el NPC.");
        }
    }*/
}
