using UnityEngine;
using UnityEngine.UI;

public class MusicalButton : MonoBehaviour
{
    public int buttonID;
    public AudioSource audioSource;
    public Image buttonImage;

    [HideInInspector] public Color originalColor;
    public Color highlightColor = Color.yellow;
    public Color errorColor = Color.red;

    private bool isErrorState = false; // Para evitar conflictos de color

    private void Start()
    {
        originalColor = buttonImage.color;
    }

    // Resaltar botón (al hacer secuencia o clic del jugador)
    public void Highlight()
    {
        if (isErrorState) return; // no sobrescribir color de error
        buttonImage.color = highlightColor;
        PlaySound();
        Invoke(nameof(ResetColor), 0.3f);
    }

    // Restaurar color original
    public void ResetColor()
    {
        if (isErrorState) return; // no sobrescribir color de error
        buttonImage.color = originalColor;
    }

    // Llamado al hacer clic
    public void OnPlayerClick()
    {
        MusicalGameManager.Instance.PlayerPress(buttonID);
        Highlight(); // Cambia a amarillo y suena
    }

    // Cambiar a estado de error (rojo)
    public void SetErrorState(bool isError)
    {
        isErrorState = isError;
        buttonImage.color = isError ? errorColor : originalColor;
    }

    // Reproducir sonido del botón
    public void PlaySound()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
