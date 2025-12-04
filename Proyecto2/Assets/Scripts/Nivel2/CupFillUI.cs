using UnityEngine;
using UnityEngine.UI;

public class CupFillUI : MonoBehaviour
{
    [Header("Referencia de imagen")]
    public Image fillImage;

    [Header("Sprites por nivel")]
    public Sprite[] levelSprites; 

    private int maxAmount;

    public void Initialize(int max)
    {
        maxAmount = max;
        UpdateFill(0);
    }

    public void UpdateFill(int amount)
    {
        float percent = (float)amount / (float)maxAmount;

        int index = Mathf.RoundToInt(percent * (levelSprites.Length - 1));
        index = Mathf.Clamp(index, 0, levelSprites.Length - 1);

        if (fillImage != null && levelSprites.Length > 0)
        {
            fillImage.sprite = levelSprites[index];
        }
    }
}
