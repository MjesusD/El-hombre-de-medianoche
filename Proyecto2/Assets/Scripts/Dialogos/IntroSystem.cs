using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSystem : MonoBehaviour
{
    [Header("UI Fade")]
    [SerializeField] private Image fadeImage;

    [Header("Audio del Menú (MusicManager)")]
    [SerializeField] private AudioSource menuMusicSource;

    [Header("Panel Narrativo")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private float musicFadeDuration = 2f;

    [Header("Texto Narrativo")]
    [TextArea(3, 5)]
    [SerializeField] private string[] textos;

    [Header("Transiciones")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Carga de Escena Final")]
    [SerializeField] private string nextSceneName;

    private int currentIndex = 0;
    private bool introFinished = false;
    private Coroutine currentCoroutine;

    private void Start()
    {
        if (introPanel != null)
            introPanel.SetActive(false);

        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 1);

        nextButton.onClick.AddListener(NextText);

        if (skipButton != null)
            skipButton.onClick.AddListener(SkipIntro);
    }

    public void StartIntro()
    {
        introFinished = false;
        currentIndex = 0;

        // Asegurar panel encendido
        introPanel.SetActive(true);

        // Ocultar el texto hasta el fade
        introText.text = "";

        // Pausar música del menú
        if (MusicManager.Instance != null)
        {
            var mmAudio = MusicManager.Instance.GetComponent<AudioSource>();
            if (mmAudio != null)
                mmAudio.Pause();
        }
        else if (menuMusicSource != null)
        {
            menuMusicSource.Pause();
        }

        // Iniciar secuencia
        currentCoroutine = StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        // Música de intro
        if (introMusic != null && musicSource != null)
        {
            musicSource.clip = introMusic;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeMusic(0f, 1f, musicFadeDuration));
        }

        // Fade in
        yield return Fade(1f, 0f);

        ShowText();
    }

    private void ShowText()
    {
        if (!introFinished && currentIndex < textos.Length)
            introText.text = textos[currentIndex];
    }

    private void NextText()
    {
        if (introFinished) return;

        currentIndex++;

        if (currentIndex >= textos.Length)
        {
            FinishIntro();
            return;
        }

        ShowText();
    }

    private void SkipIntro()
    {
        if (introFinished) return;

        FinishIntro();
    }

    private void FinishIntro()
    {
        introFinished = true;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(EndIntro());
    }

    private IEnumerator EndIntro()
    {
        yield return Fade(0f, 1f);

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float from, float to)
    {
        if (fadeImage == null)
            yield break;

        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, time / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }

    private IEnumerator FadeMusic(float from, float to, float duration)
    {
        if (musicSource == null)
            yield break;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }

        musicSource.volume = to;
    }
}
