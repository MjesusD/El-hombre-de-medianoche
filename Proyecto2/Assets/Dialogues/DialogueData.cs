using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string text;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue")]
public class DialogueData : ScriptableObject
{
    public string dialogueID;
    public bool repeatable = false;
    public DialogueLine[] lines;
}
