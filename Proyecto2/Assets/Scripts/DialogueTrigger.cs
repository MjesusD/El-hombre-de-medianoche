using UnityEngine;

public class InkTrigger : MonoBehaviour
{
    public TextAsset inkJSON;
    public string knotName = "start";
    public KeyCode interactKey = KeyCode.E;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
        {
            InkDialogueManager.Instance.StartStory(inkJSON, knotName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
