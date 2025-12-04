using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Referencias")]
    public Image frontImage;
    public Image backImage;

    [Header("Datos")]
    public int cardId;
    public bool isRevealed = false;
    public bool isMatched = false;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(int id, Sprite frontSprite)
    {
        cardId = id;
        frontImage.sprite = frontSprite;
        Hide(); // Inicialmente la carta está boca abajo
    }

    // Este método se llama desde el OnClick del Button
    public void OnCardClicked()
    {
        if (isMatched || isRevealed) return;   // no permitir tocar cartas bloqueadas o ya abiertas
        MemoryGameManager.Instance.SelectCard(this);
    }

    public void Reveal()
    {
        isRevealed = true;
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    public void Hide()
    {
        isRevealed = false;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }

    public void Lock()
    {
        isMatched = true;
        button.interactable = false; // deshabilitar botón para que no pueda volver a tocarse
    }
}
