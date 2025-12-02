using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Configuración")]
    public GameObject bubblePrefab;
    public Canvas uiCanvas;

    private GameObject currentBubble;

    void Awake()
    {
        Instance = this;
    }

  
    // Solo mensajes de interacción / burbuja
  

    public void ShowBubble(string message, Transform worldTarget)
    {
        if (bubblePrefab == null || uiCanvas == null)
        {
            Debug.LogWarning("Falta prefab o canvas");
            return;
        }

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
