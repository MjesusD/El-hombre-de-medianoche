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
    [SerializeField] private Button skipButton; // botón para skipear

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

    private void Start()
    {
        introPanel.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 1);

        nextButton.onClick.AddListener(NextText);

        if (skipButton != null)
            skipButton.onClick.AddListener(SkipIntro);
    }

    public void StartIntro()
    {
        // Pausar música del menú usando MusicManager si existe
        if (MusicManager.Instance != null)
        {
            var mmAudio = MusicManager.Instance.GetComponent<AudioSource>();
            if (mmAudio != null && mmAudio.isPlaying)
                mmAudio.Pause();
        }
        else
        {
            if (menuMusicSource != null && menuMusicSource.isPlaying)
                menuMusicSource.Pause();
        }

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        if (introMusic != null)
        {
            musicSource.clip = introMusic;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeMusic(0f, 1f, musicFadeDuration));
        }

        yield return Fade(1f, 0f);

        introPanel.SetActive(true);
        ShowText();
    }

    private void ShowText()
    {
        introText.text = textos[currentIndex];
    }

    private void NextText()
    {
        currentIndex++;

        if (currentIndex >= textos.Length)
        {
            StartCoroutine(EndIntro());
            return;
        }

        ShowText();
    }

    // Función para skip
    private void SkipIntro()
    {
        StopAllCoroutines();
        StartCoroutine(EndIntro());
    }

    private IEnumerator EndIntro()
    {
        yield return Fade(0f, 1f);

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float from, float to)
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, time / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }

    private IEnumerator FadeMusic(float from, float to, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }
    }
}
