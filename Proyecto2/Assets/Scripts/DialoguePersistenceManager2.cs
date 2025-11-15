using System.Collections.Generic;
using UnityEngine;

public class DialoguePersistenceManager2 : MonoBehaviour
{
    public static DialoguePersistenceManager2 Instance;

    // Lista simple de diálogos vistos
    private HashSet<string> seenDialogues = new HashSet<string>();

    private void Awake()
    {
        // Singleton simple
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasSeen(string dialogueID)
    {
        return seenDialogues.Contains(dialogueID);
    }

    public void MarkSeen(string dialogueID)
    {
        if (!seenDialogues.Contains(dialogueID))
            seenDialogues.Add(dialogueID);
    }
}
