using UnityEngine;

public class InkDialogueState : MonoBehaviour
{
    public TextAsset inkJSON;
    public string currentKnot = "inicio"; // primer diálogo

    public void SetKnot(string newKnot)
    {
        currentKnot = newKnot;
    }
}
