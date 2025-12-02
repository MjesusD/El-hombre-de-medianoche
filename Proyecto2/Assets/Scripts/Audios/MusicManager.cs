using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    [Header("Música por escena")]
    public SceneMusic[] sceneMusics;

    [Header("Fade Settings")]
    public float fadeDuration = 1.2f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            Debug.LogError("MusicManager necesita un AudioSource.");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        AudioClip clip = GetClipForScene(scene.name);

        if (clip != null)
        {
            // Si la música es distinta hacer fade
            if (audioSource.clip != clip)
                StartFadeToNewClip(clip);
            else
            {
                // Si es la misma pero estaba detenida
                if (!audioSource.isPlaying)
                    StartFadeIn();
            }
        }
        else
        {
            // No hay música asignada fade out
            StartFadeOut();
        }
    }

    // --- Buscar música asignada a la escena ---
    private AudioClip GetClipForScene(string sceneName)
    {
        foreach (SceneMusic sm in sceneMusics)
        {
            if (sm.sceneName == sceneName)
                return sm.musicClip;
        }
        return null;
    }

    // --- Fades ---
    private void StartFadeToNewClip(AudioClip newClip)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToNewClip(newClip));
    }

    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        // Fade out
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;

        // Cambiar clip
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }

    private void StartFadeOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Pause();
    }

    private void StartFadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = 1f;
        audioSource.volume = 0f;
        audioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }


    // --- CONTROLES GLOBALES DE PAUSA / RESUME ---

    // Pausar música sin fade
    public void PauseMusic()
    {
        if (audioSource != null)
            audioSource.Pause();
    }

    // Reanudar música sin fade
    public void ResumeMusic()
    {
        if (audioSource != null)
            audioSource.UnPause();

    }
}
