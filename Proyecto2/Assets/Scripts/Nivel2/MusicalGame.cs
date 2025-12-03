using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicalGame : PuzzleBase
{
    [Header("Puzzle")]
    public List<MusicalButton> buttons;     // Lista de botones disponibles
    public int sequenceLength = 4;

    private List<int> sequence = new List<int>();   // La canción generada
    private int playerIndex = 0;
    private bool inputEnabled = false;

    public override void StartPuzzle()
    {
         Debug.Log("StartPuzzle llamado correctamente.");
        //puzzlePanel.SetActive(true);
        base.StartPuzzle();
        GenerateSequence();
        StartCoroutine(PlaySequence());
    }

    void GenerateSequence()
    {
        sequence.Clear();

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomId = Random.Range(0, buttons.Count);
            sequence.Add(randomId);
        }
    }

    IEnumerator PlaySequence()
    {
        inputEnabled = false;
        yield return new WaitForSeconds(0.5f);

        foreach (int id in sequence)
        {
            buttons[id].Flash();
            yield return new WaitForSeconds(0.6f);
        }

        inputEnabled = true;
        playerIndex = 0;
    }

    public void PlayerPress(int buttonId)
    {

        Debug.Log("Presionaste botón: " + buttonId);
        if (!inputEnabled) return;

        // Si presionó correctamente
        if (buttonId == sequence[playerIndex])
        {
            playerIndex++;

            // Si completó toda la secuencia, puzzle ganado
            if (playerIndex >= sequence.Count)
            {
                Debug.Log("SECUENCIA COMPLETADA");   // AÑADE ESTO
                CompletePuzzle();
            }

        }
        else
        {
            // ERROR — Reiniciar puzzle completo
            Debug.Log("Fallaste. Reiniciando secuencia...");
            StartCoroutine(RestartPuzzle());
        }
    }

    IEnumerator RestartPuzzle()
    {
        inputEnabled = false;
        yield return new WaitForSeconds(0.4f);

        GenerateSequence();
        yield return PlaySequence();
 
    }

}
