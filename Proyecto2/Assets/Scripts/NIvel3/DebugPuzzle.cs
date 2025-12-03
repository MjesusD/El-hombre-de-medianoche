using UnityEngine;

public class DebugPuzzle : MonoBehaviour
{
    [Header("Para Testing")]
    [SerializeField] private PuzzleTuberias puzzle;

    void Update()
    {
        // Presiona F1 para abrir el puzzle sin llave (bypass)
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("=== FORZANDO APERTURA DEL PUZZLE ===");
            if (puzzle != null)
            {
                puzzle.AbrirPuzzle();
            }
        }

        // Presiona F2 para mostrar info del grid
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("=== INFORMACIÓN DEL PUZZLE ===");
            MostrarInfoPuzzle();
        }
    }

    void MostrarInfoPuzzle()
    {
        if (puzzle == null)
        {
            Debug.LogError("No hay referencia a PuzzleTuberias");
            return;
        }

        Debug.Log($"Puzzle asignado: {puzzle.name}");
        Debug.Log($"¿Está resuelto? {puzzle.EstáResuelto()}");
    }
}