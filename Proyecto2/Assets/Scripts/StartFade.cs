using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class StartFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        if (fadeImage != null)
        {
            // Asegurar que comience totalmente negro
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;

            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("SceneStartFader no tiene asignado un fadeImage.");
        }
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
