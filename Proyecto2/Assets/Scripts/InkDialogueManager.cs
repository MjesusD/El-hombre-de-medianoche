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

    void Awake()
    {
        // Singleton simple (no persistente)
        if (Instance == null) Instance = this;
        else Instance = this;

        // Safety checks
        if (nextButton == null) Debug.LogWarning("[InkDialogueManager] nextButton NO asignado en el Inspector.");
        if (dialoguePanel == null) Debug.LogWarning("[InkDialogueManager] dialoguePanel NO asignado en el Inspector.");
        if (dialogueText == null) Debug.LogWarning("[InkDialogueManager] dialogueText NO asignado en el Inspector.");
        if (choicesContainer == null) Debug.LogWarning("[InkDialogueManager] choicesContainer NO asignado en el Inspector.");
        if (choiceButtonPrefab == null) Debug.LogWarning("[InkDialogueManager] choiceButtonPrefab NO asignado en el Inspector.");

        // Evitar que el botón acumule listeners, sólo si existe
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(ContinueStory);
        }

        // Ocultar panel al inicio si está asignado
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    // Inicio diálogo
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
            // Elegir path pero lo hacemos con try-catch por si no existe
            try
            {
                story.ChoosePathString(knot);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[InkDialogueManager] ChoosePathString falló: " + ex.Message);
            }
        }

        playing = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        Debug.Log("[InkDialogueManager] Story iniciada. canContinue: " + story.canContinue + " choices: " + story.currentChoices.Count);

        ContinueStory();
    }

    // Avanzar diálogo 
    public void ContinueStory()
    {
        Debug.Log("[InkDialogueManager] ContinueStory llamado.");

        if (story == null)
        {
            Debug.LogWarning("[InkDialogueManager] ContinueStory: story es null.");
            return;
        }

        ClearChoices();

        try
        {
            if (story.canContinue)
            {
                if (nextButton != null) nextButton.gameObject.SetActive(true);

                string newText = story.Continue().Trim();
                if (dialogueText != null) dialogueText.text = newText;
                Debug.Log("[InkDialogueManager] Texto mostrado: " + newText);

                HandleTags(story.currentTags);
            }
            else if (story.currentChoices.Count > 0)
            {
                if (nextButton != null) nextButton.gameObject.SetActive(false);
                DisplayChoices();
            }
            else
            {
                EndStory();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[InkDialogueManager] Excepción en ContinueStory: " + ex.Message + "\n" + ex.StackTrace);
        }
    }

    // Mostrar choices
    void DisplayChoices()
    {
        if (choicesContainer == null || choiceButtonPrefab == null)
        {
            Debug.LogWarning("[InkDialogueManager] DisplayChoices: falta choicesContainer o choiceButtonPrefab.");
            return;
        }

        if (story == null)
        {
            Debug.LogWarning("[InkDialogueManager] DisplayChoices: story es null.");
            return;
        }

        nextButton?.gameObject.SetActive(false);

        List<Choice> choices = story.currentChoices;
        Debug.Log("[InkDialogueManager] DisplayChoices count: " + choices.Count);

        foreach (var choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer);
            if (buttonObj == null) continue;

            TMP_Text txt = buttonObj.GetComponentInChildren<TMP_Text>();
            if (txt != null) txt.text = choice.text;
            else Debug.LogWarning("[InkDialogueManager] El prefab de choice no tiene TMP_Text en hijos.");

            Button btn = buttonObj.GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogWarning("[InkDialogueManager] El prefab de choice NO tiene componente Button.");
                continue;
            }

            int choiceIndex = choice.index; // importante: variable local para evitar captura incorrecta
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => {
                Debug.Log("[InkDialogueManager] Choice pulsado: index=" + choiceIndex + " text=" + choice.text);
                ChooseChoice(choiceIndex);
            });
        }
    }

    void ChooseChoice(int index)
    {
        if (story == null)
        {
            Debug.LogWarning("[InkDialogueManager] ChooseChoice: story es null.");
            return;
        }

        Debug.Log("[InkDialogueManager] ChooseChoice: " + index);
        story.ChooseChoiceIndex(index);
        ContinueStory();
    }

    // Limpiar
    void ClearChoices()
    {
        if (choicesContainer == null) return;
        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(choicesContainer.GetChild(i).gameObject);
        }
    }
    public static System.Action<List<string>> OnTagsReceived;

    // Tags opcionales
    void HandleTags(List<string> tags)
    {
        if (tags == null || tags.Count == 0) return;

        foreach (string tag in tags)
        {
            Debug.Log("[InkDialogueManager] TAG: " + tag);
        }

        // Enviar a inkDialogue
        OnTagsReceived?.Invoke(tags);
    }

    // Fin diálogo 
    void EndStory()
    {
        playing = false;

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        ClearChoices();
        Debug.Log("[InkDialogueManager] EndStory ejecutado.");
    }

    public bool IsPlaying() => playing;

    // Variables de Ink
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
    

}
