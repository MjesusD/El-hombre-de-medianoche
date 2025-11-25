using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class InkDialogueSystem : MonoBehaviour
{
    public static InkDialogueSystem Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Button nextButton;

    private Story story;
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            dialoguePanel.SetActive(false);
            nextButton.onClick.AddListener(NextLine);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Cargar un nuevo JSON cuando un NPC use un archivo diferente
    public void LoadNewStory(TextAsset inkJSON)
    {
        story = new Story(inkJSON.text);
    }

    // Iniciar un diálogo desde un knot específico
    public void StartDialogue(string knotName)
    {
        if (story == null)
        {
            Debug.LogError("No se cargó ningún archivo .ink.json en InkDialogueSystem");
            return;
        }

        if (isPlaying) return;

        // Seleccionar el knot
        if (!string.IsNullOrEmpty(knotName))
            story.ChoosePathString(knotName);

        dialoguePanel.SetActive(true);
        isPlaying = true;
        NextLine();
    }

    private void NextLine()
    {
        if (story.canContinue)
        {
            dialogueText.text = story.Continue().Trim();
        }
        else
        {
            // fin del diálogo
            dialoguePanel.SetActive(false);
            isPlaying = false;
        }
    }

    public bool IsPlaying() => isPlaying;

    public Story GetStory() => story;
}
