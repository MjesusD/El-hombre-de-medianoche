using UnityEngine;

public class PuzzleBase : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzlePanel; // panel con el puzzle

    [Header("Desbloqueo")]
    public GameObject hiddenObject;            // objeto oculto a activar
    public MonoBehaviour interactionToUnlock;  // InteractionObject a activar

    private bool completed = false;

    public virtual void StartPuzzle()
    {
        puzzlePanel.SetActive(true);
    }

    public virtual void CompletePuzzle()
    {
        if (completed) return;
        completed = true;

        // Cerrar panel
        puzzlePanel.SetActive(false);

        // Activar objeto oculto
        if (hiddenObject != null)
            hiddenObject.SetActive(true);

        // Activar interacción
        if (interactionToUnlock != null)
            interactionToUnlock.enabled = true;

        Debug.Log("Puzzle completado y desbloqueado.");
    }
}
