using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using System.Collections.Generic;

public class InkDialogueManager : MonoBehaviour
{
    public static InkDialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Transform choicesContainer;
    public Button nextButton;
    public GameObject choiceButtonPrefab;

    private Story story;
    private bool playing = false;

    // -------------------------------
    // ENTREGA DE ITEMS
    // -------------------------------
    private bool waitingForItem = false;
    private string expectedItemName = "";
    private string itemDeliveredKnot = "";
    private bool consumeItem = true;

    private InkTrigger currentTrigger;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(ContinueStory);
        }

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    // =================================================
    // INICIO DE HISTORIA
    // =================================================
    public void StartStory(TextAsset inkJSON, string knot = "")
    {
        if (inkJSON == null)
        {
            Debug.LogError("[InkDialogueManager] StartStory llamado con inkJSON = null");
            return;
        }

        story = new Story(inkJSON.text);

        if (!string.IsNullOrEmpty(knot))
        {
            try { story.ChoosePathString(knot); }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[InkDialogueManager] ChoosePathString falló: " + ex.Message);
            }
        }

        playing = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        ContinueStory();
    }

    // =================================================
    // CONTINUAR HISTORIA
    // =================================================
    public void ContinueStory()
    {
        if (story == null) return;

        ClearChoices();

        try
        {
            if (story.canContinue)
            {
                nextButton?.gameObject.SetActive(true);

                string newText = story.Continue().Trim();
                if (dialogueText != null) dialogueText.text = newText;

                HandleTags(story.currentTags);
            }
            else if (story.currentChoices.Count > 0)
            {
                nextButton?.gameObject.SetActive(false);
                DisplayChoices();
            }
            else
            {
                EndStory();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[InkDialogueManager] Excepción en ContinueStory: " + ex.Message);
        }
    }

    // =================================================
    // OPCIONES
    // =================================================
    void DisplayChoices()
    {
        List<Choice> choices = story.currentChoices;

        foreach (var choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer);
            TMP_Text txt = buttonObj.GetComponentInChildren<TMP_Text>();
            if (txt != null) txt.text = choice.text;

            Button btn = buttonObj.GetComponent<Button>();
            int index = choice.index;
            btn.onClick.AddListener(() => ChooseChoice(index));
        }
    }

    void ChooseChoice(int index)
    {
        story.ChooseChoiceIndex(index);
        ContinueStory();
    }

    void ClearChoices()
    {
        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
            Destroy(choicesContainer.GetChild(i).gameObject);
    }

    // =================================================
    // TAGS
    // =================================================
    public static System.Action<List<string>> OnTagsReceived;

    void HandleTags(List<string> tags)
    {
        if (tags == null) return;

        foreach (string tag in tags)
        {
            Debug.Log("[InkDialogueManager] TAG: " + tag);

            // ejemplo: "darObjeto"
            if (tag == "darObjeto")
            {
                waitingForItem = true;
                nextButton.gameObject.SetActive(false);
                Inventario.Instance.ToggleInventory();
            }
        }

        OnTagsReceived?.Invoke(tags);
    }

    // =================================================
    // FIN DE HISTORIA
    // =================================================
    void EndStory()
    {
        playing = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        ClearChoices();
    }

    public bool IsPlaying() => playing;

    // =================================================
    // VARIABLES INK
    // =================================================
    public object GetVariable(string variableName)
    {
        if (story == null) return null;
        return story.variablesState[variableName];
    }

    public void SetVariable(string variableName, object value)
    {
        if (story == null) return;
        story.variablesState[variableName] = value;
    }

    // =================================================
    // ITEM EXPECTATION (INTEGRADO)
    // =================================================
    public void ExpectItem(string itemName, bool consume, string knotToJump)
    {
        expectedItemName = itemName;
        consumeItem = consume;
        itemDeliveredKnot = knotToJump;
        waitingForItem = true;
    }

    public bool IsWaitingForItem() => waitingForItem;

    public bool TryGiveItem(string itemName)
    {
        if (!waitingForItem) return false;

        if (itemName == expectedItemName)
        {
            Debug.Log("Objeto correcto entregado al NPC");

            waitingForItem = false;

            // cerrar inventario
            Inventario.Instance.ToggleInventory();

            // continuar diálogo
            ContinueStory();

            return true;
        }
        else
        {
            Debug.Log("Este no es el objeto que necesita el NPC");
            return false;
        }
    }


    // =================================================
    // TRIGGER ACTUAL (por si necesitas lógica extra)
    // =================================================
    public void SetCurrentTrigger(InkTrigger trigger) => currentTrigger = trigger;

    public void SetExpectedItem(string item) => expectedItemName = item;

}
