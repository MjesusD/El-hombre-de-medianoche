using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{
    public Image image;
    public Button button;

    public int currentID;   // sprite actual
    private PuzzleImageManager manager;

    private bool selected = false;

    public void Setup(int id, Sprite sprite, PuzzleImageManager mgr)
    {
        currentID = id;
        image.sprite = sprite;
        manager = mgr;

        button.onClick.AddListener(OnClick);
    }

    public void SetData(int id, Sprite sprite)
    {
        currentID = id;
        image.sprite = sprite;
    }

    public void OnClick()
    {
        manager.SelectPiece(this);
    }

    public void SelectVisual(bool active)
    {
        selected = active;
        image.color = active ? Color.yellow : Color.white;
    }
}
