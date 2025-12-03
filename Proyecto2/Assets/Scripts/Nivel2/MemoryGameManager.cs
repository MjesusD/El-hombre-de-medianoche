using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryGameManager : MonoBehaviour
{
    public static MemoryGameManager Instance;

    [Header("Configuración")]
    public Transform gridParent;         // El GridLayoutGroup
    public Card cardPrefab;              // Prefab de la carta
    public List<Sprite> cardSprites;     // Lista de sprites únicos (uno por pareja)
    public int pairs = 4;                // Cantidad de pares

    private List<Card> cards = new List<Card>();
    private Card firstCard;
    private Card secondCard;
    private bool canClick = true;

    [Header("UI")]
    public GameObject puzzlePanel;       // El panel que contiene el grid
    public GameObject hiddenObject;      // El objeto oculto que se desbloquea

    [Header("Interacción")]
    public MonoBehaviour interactionToUnlock;   // El script de interaction que estaba desactivado


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateCards();
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

        // Mezclar la lista
        for (int i = 0; i < ids.Count; i++)
        {
            int rnd = Random.Range(0, ids.Count);
            (ids[i], ids[rnd]) = (ids[rnd], ids[i]);
        }

        // Instanciar cartas según el ID mezclado
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

            // Comprobar si todas las cartas están emparejadas
            if (AllCardsMatched())
            {
                yield return new WaitForSeconds(0.3f);

                // Cerrar panel del puzzle
                puzzlePanel.SetActive(false);

                // Desbloquear objeto oculto
                hiddenObject.SetActive(true);

                // Activar script Interaction
                if (interactionToUnlock != null)
                    interactionToUnlock.enabled = true;
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
