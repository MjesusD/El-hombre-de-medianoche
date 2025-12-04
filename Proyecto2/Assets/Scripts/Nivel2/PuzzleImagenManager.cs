using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PuzzleImageManager : PuzzleBase
{
    [Header("Puzzle Config")]
    public GridLayoutGroup grid;             // Contenedor ordenado
    public PuzzlePiece piecePrefab;     // Prefab con botón + imagen
    public List<Sprite> sprites;             // Sprites en orden correcto

    private List<PuzzlePiece> pieces = new List<PuzzlePiece>();
    private PuzzlePiece firstSelected;
    private bool canClick = true;

    private void Start()
    {
        RefreshUIFader();
        puzzlePanel.SetActive(false);
    }

    public override void StartPuzzle()
    {
        GeneratePieces();
        base.StartPuzzle();
    }

    void GeneratePieces()
    {
        // Limpiar si ya existían piezas
        foreach (Transform t in grid.transform)
            Destroy(t.gameObject);

        pieces.Clear();

        // Crear una lista mezclada de índices
        List<int> ids = new List<int>();
        for (int i = 0; i < sprites.Count; i++)
            ids.Add(i);

        // Mezclar
        for (int i = 0; i < ids.Count; i++)
        {
            int rnd = Random.Range(0, ids.Count);
            (ids[i], ids[rnd]) = (ids[rnd], ids[i]);
        }

        // Instanciar piezas mezcladas
        foreach (int id in ids)
        {
            PuzzlePiece newPiece = Instantiate(piecePrefab, grid.transform);
            newPiece.Setup(id, sprites[id], this);
            pieces.Add(newPiece);
        }
    }

    public void SelectPiece(PuzzlePiece piece)
    {
        if (!canClick) return;

        // Primera selección
        if (firstSelected == null)
        {
            firstSelected = piece;
            piece.SelectVisual(true);
            return;
        }

        // Segunda selección
        piece.SelectVisual(true);

        canClick = false;
        SwapPieces(firstSelected, piece);

        firstSelected.SelectVisual(false);
        piece.SelectVisual(false);

        firstSelected = null;

        canClick = true;

        // Chequear si está resuelto
        if (IsPuzzleSolved())
            CompletePuzzle();
    }

    void SwapPieces(PuzzlePiece a, PuzzlePiece b)
    {
        int tempID = a.currentID;
        Sprite tempSprite = a.image.sprite;

        a.SetData(b.currentID, b.image.sprite);
        b.SetData(tempID, tempSprite);
    }

    bool IsPuzzleSolved()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].currentID != i)
                return false;
        }
        return true;
    }
}
