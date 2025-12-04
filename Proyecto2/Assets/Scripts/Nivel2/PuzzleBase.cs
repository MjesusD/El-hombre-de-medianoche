using UnityEngine;

public class PuzzleBase : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzlePanel; // panel con el puzzle
    private UIFader uiFader;      

    [Header("Desbloqueo")]
    public GameObject hiddenObject;            // objeto oculto a activar
    public MonoBehaviour interactionToUnlock;  // InteractionObject a activar

    [Header("Inicio del Puzzle")]
    [SerializeField] public MonoBehaviour puzzleStarter;

    [Header("Cadena de Puzzles")]
    public InteractionObject nextInteractionObject;

    private bool completed = false;

    public void RefreshUIFader()
    {
        if (puzzlePanel != null)
        {
            uiFader = puzzlePanel.GetComponent<UIFader>();
            if (uiFader == null)
                uiFader = puzzlePanel.GetComponentInChildren<UIFader>(true);
        }
    }



    public virtual void StartPuzzle()
    {
        //  fade in
        if (uiFader != null)
            uiFader.FadeIn();
        else
            puzzlePanel.SetActive(true);
    }

    public virtual void CompletePuzzle()
    {
        if (completed) return;
        completed = true;

        // Fade out o cerrar panel
        if (uiFader != null)
            uiFader.FadeOutAndDisable();
        else
            puzzlePanel.SetActive(false);

        // Activar objeto oculto
        if (hiddenObject != null)
            hiddenObject.SetActive(true);

        // Desactivar interaction del puzzle que acabó
        if (puzzleStarter != null)
        {
            InteractionObject io = puzzleStarter.GetComponent<InteractionObject>();
            if (io != null)
                io.DisableInteraction();
        }

        // Activar interacción adicional, si existe
        if (interactionToUnlock != null)
        {
            interactionToUnlock.enabled = true;

            InteractionObject io = interactionToUnlock.GetComponent<InteractionObject>();
            if (io != null)
                io.UnlockPuzzlePrompt();
        }

        // ACTIVAR EL SIGUIENTE OBJETO DEL PUNTAJE
        if (nextInteractionObject != null)
        {
            nextInteractionObject.EnableInteractionFromPuzzle();
            nextInteractionObject.UnlockPuzzlePrompt();   
        }

        Debug.Log("Puzzle completado y cadena actualizada.");
    }

}
