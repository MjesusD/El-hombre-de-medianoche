using UnityEngine;

public class CoffeePuzzle : PuzzleBase
{
    [Header("Coffee Puzzle")]
    public int[] cups = new int[4];          // niveles actuales (0–4)
    public int[] maxCapacity = new int[4];   // normalmente: {4, 4, 4, 4}

    private int? selectedCup = null;         // taza actualmente seleccionada

    public CupFillUI[] cupFillUIs;


    private void Start()
    {
        RefreshUIFader();
        puzzlePanel.SetActive(false);
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();

        // Inicializar UI de tazas
        for (int i = 0; i < cups.Length; i++)
        {
            cupFillUIs[i].Initialize(maxCapacity[i]);
            cupFillUIs[i].UpdateFill(cups[i]);
        }
    }


    // Seleccionar taza para verter
    public void SelectCup(int index)
    {
        if (selectedCup == null)
        {
            selectedCup = index;
        }
        else
        {
            Pour(selectedCup.Value, index);
            selectedCup = null;
        }
    }


    // --- TRASPASO DE NIVELES DISCRETOS ---
    public void Pour(int from, int to)
    {
        if (from == to) return;

        // No se puede verter si la de destino está llena
        if (cups[to] >= maxCapacity[to])
            return;

        // No se puede verter si la de origen está vacía
        if (cups[from] <= 0)
            return;

        // Verter 1 nivel (equivale a 25%)
        cups[from] -= 1;
        cups[to] += 1;

        // Nunca pasar el maxCapacity
        if (cups[to] > maxCapacity[to])
            cups[to] = maxCapacity[to];

        // Actualizar UI
        cupFillUIs[from].UpdateFill(cups[from]);
        cupFillUIs[to].UpdateFill(cups[to]);

        Debug.Log($"Vertido: 1 nivel de taza {from} a taza {to}");

        CheckIfSolved();
    }


    // --- CONDICIÓN DE VICTORIA ---
    private void CheckIfSolved()
    {
        int reference = cups[0];

        for (int i = 1; i < cups.Length; i++)
        {
            if (cups[i] != reference)
                return; // si una taza difiere, aún no ganas
        }

        Debug.Log("Puzzle de café resuelto: todas las tazas tienen el mismo nivel.");
        CompletePuzzle();
    }
}
