using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource panelAudio;

    [Header("UI")]
    [SerializeField] private Button playAudioButton;
    [SerializeField] private Button closePanelButton;

    private void Start()
    {
        if (playAudioButton != null)
            playAudioButton.onClick.AddListener(PlayPanelAudio);

        if (closePanelButton != null)
            closePanelButton.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        // Pausar música global
        MusicManager.Instance?.PauseMusic();
    }

    private void OnDisable()
    {
        // Resetear audio del panel
        if (panelAudio != null)
        {
            panelAudio.Stop();
            panelAudio.time = 0;
        }

        // Reanudar música global
        MusicManager.Instance?.ResumeMusic();
    }

    private void PlayPanelAudio()
    {
        if (panelAudio != null)
        {
            panelAudio.Stop();
            panelAudio.time = 0;
            panelAudio.Play();
        }
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
