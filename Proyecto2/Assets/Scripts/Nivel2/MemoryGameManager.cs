using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryGameManager : PuzzleBase
{
    public static MemoryGameManager Instance;

    [Header("Configuración")]
    public Transform gridParent;
    public Card cardPrefab;
    public List<Sprite> cardSprites;
    public int pairs = 4;

    private List<Card> cards = new List<Card>();
    private Card firstCard;
    private Card secondCard;
    private bool canClick = true;

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {

        RefreshUIFader();
        puzzlePanel.SetActive(false);   // Ocultar puzzle al inicio
        
    }

    public override void StartPuzzle()
    {

        ClearCards();
        GenerateCards();
        base.StartPuzzle();
       
    }

    void ClearCards()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        cards.Clear();
    }


    void GenerateCards()
    {
        List<int> ids = new List<int>();

        // Crear 2 copias de cada ID (pares)
        for (int i = 0; i < pairs; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        // Mezclar
        for (int i = 0; i < ids.Count; i++)
        {
            int rnd = Random.Range(0, ids.Count);
            (ids[i], ids[rnd]) = (ids[rnd], ids[i]);
        }

        // Instanciar cartas
        foreach (int id in ids)
        {
            Card newCard = Instantiate(cardPrefab, gridParent);
            newCard.Setup(id, cardSprites[id]);
            cards.Add(newCard);
        }
    }

    public void SelectCard(Card card)
    {
        if (!canClick) return;

        card.Reveal();

        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            canClick = false;
            StartCoroutine(CheckPair());
        }
    }

    IEnumerator CheckPair()
    {
        yield return new WaitForSeconds(0.7f);

        if (firstCard.cardId == secondCard.cardId)
        {
            firstCard.Lock();
            secondCard.Lock();

            // Todas las cartas emparejadas?
            if (AllCardsMatched())
            {
                yield return new WaitForSeconds(0.3f);

                // Puzzle completado
                CompletePuzzle();
            }
        }
        else
        {
            firstCard.Hide();
            secondCard.Hide();
        }

        firstCard = null;
        secondCard = null;
        canClick = true;
    }

    bool AllCardsMatched()
    {
        foreach (var card in cards)
        {
            if (!card.isMatched)
                return false;
        }
        return true;
    }
}
