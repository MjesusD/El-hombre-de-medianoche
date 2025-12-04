using UnityEngine;
using System.Collections.Generic;

public class PuzzleProgressManager : MonoBehaviour
{
    public static PuzzleProgressManager Instance;

    // Guarda los puzzles completados usando sus IDs
    private HashSet<string> completedPuzzles = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool IsCompleted(string puzzleID)
    {
        return completedPuzzles.Contains(puzzleID);
    }

    public void MarkCompleted(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
            completedPuzzles.Add(puzzleID);
    }
}
