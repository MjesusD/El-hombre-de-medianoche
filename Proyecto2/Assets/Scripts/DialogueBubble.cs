using UnityEngine;
using TMPro;

public class DialogueBubble : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public Vector3 offset = new Vector3(0, 2f, 0);

    private RectTransform rectTransform;
    private Transform target;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (messageText == null)
            messageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(string message, Transform worldTarget)
    {
        target = worldTarget;
        messageText.text = message;
        UpdatePosition();
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (target == null || Camera.main == null) return;

        // Convertir posición mundial a pantalla
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);

        // Si el target está detrás de la cámara ? ocultar burbuja
        if (screenPos.z < 0)
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);

            return;
        }

        // Reactivar si vuelve al campo de visión
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        rectTransform.position = screenPos;
    }
}
