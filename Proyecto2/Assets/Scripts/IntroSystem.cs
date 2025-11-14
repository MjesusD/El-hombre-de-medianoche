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
    // Solo se usa si no existe MusicManager

    [Header("Panel Narrativo")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private Button nextButton;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private float musicFadeDuration = 2f;

    [Header("Texto Narrativo")]
    [TextArea(3, 5)]
    [SerializeField] private string[] textos;
    [SerializeField] private float autoNextTime = 6f;

    [Header("Transiciones")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Carga de Escena Final")]
    [SerializeField] private string nextSceneName;

    private int currentIndex = 0;
    private bool currentIndexChanged = false;

    private void Start()
    {
        introPanel.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 1);
        nextButton.onClick.AddListener(NextText);
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
            // Respaldo si no estás usando MusicManager
            if (menuMusicSource != null && menuMusicSource.isPlaying)
                menuMusicSource.Pause();
        }

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        // Reproducir música propia de la intro
        if (introMusic != null)
        {
            musicSource.clip = introMusic;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeMusic(0f, 1f, musicFadeDuration));
        }

        // Fade desde negro
        yield return Fade(1f, 0f);

        // Mostrar panel narrativo
        introPanel.SetActive(true);
        ShowText();
    }

    private void ShowText()
    {
        introText.text = textos[currentIndex];
        StartCoroutine(AutoNextCoroutine());
    }

    private IEnumerator AutoNextCoroutine()
    {
        float timer = 0f;
        currentIndexChanged = false;

        while (timer < autoNextTime)
        {
            timer += Time.deltaTime;
            yield return null;

            if (currentIndexChanged)
                yield break;
        }

        NextText();
    }

    private void NextText()
    {
        currentIndexChanged = true;

        currentIndex++;
        if (currentIndex >= textos.Length)
        {
            StartCoroutine(EndIntro());
            return;
        }

        ShowText();
    }

    private IEnumerator EndIntro()
    {
        // Fade a negro
        yield return Fade(0f, 1f);

        // Guardamos que la intro ya fue vista
        PlayerPrefs.SetInt("IntroPlayed", 1);
        PlayerPrefs.Save();

        // Cargar escena final
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No se asignó la escena final en IntroSystem.");
        }
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
