using UnityEngine;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    public void FadeOutAndDisable()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float t = 0;

        // No permitir clicks durante el fade
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;

        
        // Dejar invisible
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        canvasGroup.alpha = 0;

        float t = 0;

        // No permitir interacción hasta completar el FadeIn
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1;

        // Activar interacción al finalizar el fade
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
