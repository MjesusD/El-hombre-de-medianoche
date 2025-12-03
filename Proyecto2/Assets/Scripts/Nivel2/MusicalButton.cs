using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MusicalButton : MonoBehaviour
{
    public int buttonId;
    public MusicalGame puzzle;

    [Header("Efectos")]
    public Image buttonImage;
    public Color flashColor = Color.yellow;
    public float flashTime = 0.25f;

    private Color normalColor;

    private void Start()
    {
        normalColor = buttonImage.color;
    }

    public void OnClick()
    {
        puzzle.PlayerPress(buttonId);
        StartCoroutine(FlashEffect());
    }

    public void Flash()
    {
        StartCoroutine(FlashEffect());
    }

    IEnumerator FlashEffect()
    {
        buttonImage.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        buttonImage.color = normalColor;
    }
}
