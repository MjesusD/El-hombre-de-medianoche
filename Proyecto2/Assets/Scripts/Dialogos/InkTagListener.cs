using System.Collections.Generic;
using UnityEngine;

public class InkTagListener : MonoBehaviour
{
    public string requiredItem = "piezaOrganillero"; // el objeto que necesita
    public string continueKnot = "organillero_entregar_objeto"; // knot que continúa la historia
    public InkTrigger npcTrigger; // referencia al NPC

    private bool waitingForItem = false;

    void OnEnable()
    {
        InkDialogueManager.OnTagsReceived += OnTag;
    }

    void OnDisable()
    {
        InkDialogueManager.OnTagsReceived -= OnTag;
    }

    void OnTag(List<string> tags)
    {
        foreach (string tag in tags)
        {
            if (tag == "darObjeto")
            {
                waitingForItem = true;
                Debug.Log("Esperando que el jugador entregue el objeto al NPC");
            }
        }
    }

    // Este método lo llamará el inventario
    public void TryGiveItem(string itemID)
    {
        if (!waitingForItem) return;

        if (itemID == requiredItem)
        {
            Debug.Log("OBJETO ENTREGADO AL NPC");

            waitingForItem = false;

            // Cambia el diálogo del NPC al nuevo knot
            npcTrigger.firstKnot = continueKnot;
            npcTrigger.repeatKnot = continueKnot;

            // Opcional: remueve el objeto del inventario
            // Inventory.Remove(itemID);

            // Continuar el diálogo inmediatamente:
            InkDialogueManager.Instance.ContinueStory();
        }
        else
        {
            Debug.Log("El objeto no es el correcto.");
        }
    }
}
