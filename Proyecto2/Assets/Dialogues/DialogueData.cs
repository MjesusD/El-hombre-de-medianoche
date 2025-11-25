using UnityEngine;

[CreateAssetMenu(fileName = "InkDialogue", menuName = "Dialogue/Ink Dialogue")]
public class InkDialogue : ScriptableObject
{
    public string dialogueID;   // para persistencia
    public bool repeatable = false;
    public string knotName;     // el nombre del nodo en el .ink
}
