using UnityEngine;
using UnityEngine.UI;

public class MusicalButton : MonoBehaviour
{
    public int buttonID;
    public AudioSource audioSource;
    public Image buttonImage;

    private Color originalColor;
    public Color highlightColor = Color.yellow;

    private void Start()
    {
        originalColor = buttonImage.color;
    }

    public void Highlight()
    {
        buttonImage.color = highlightColor;
        audioSource.Play();
        Invoke(nameof(ResetColor), 0.3f);
    }

    void ResetColor()
    {
        buttonImage.color = originalColor;
    }

    public void OnPlayerClick()
    {
        MusicalGameManager.Instance.PlayerPress(buttonID);
        Highlight();
    }
}
