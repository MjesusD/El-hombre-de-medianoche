using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Configuraci�n")]
    public GameObject bubblePrefab;
    public Canvas uiCanvas;

    private GameObject currentBubble;

    void Awake()
    {
        Instance = this;
    }

    public void ShowBubble(string message, Transform worldTarget)
    {
        if (bubblePrefab == null || uiCanvas == null)
        {
            Debug.LogWarning("Falta prefab o canvas");
            return;
        }

        // Si ya hay una burbuja, destruir antes
        if (currentBubble != null)
            Destroy(currentBubble);

        currentBubble = Instantiate(bubblePrefab, uiCanvas.transform);

        var bubbleScript = currentBubble.GetComponent<DialogueBubble>();
        bubbleScript.Setup(message, worldTarget);
    }

    public void HideBubble()
    {
        if (currentBubble != null)
            Destroy(currentBubble);
    }
}
