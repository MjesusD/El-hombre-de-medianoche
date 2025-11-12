using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera uiCamera;

    [Header("Jugador y HUD")]
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private GameObject playerHUD;

    private bool isInUICamera = false;

    public void SwitchToUICamera()
    {
        if (isInUICamera) return;

        mainCamera.gameObject.SetActive(false);
        uiCamera.gameObject.SetActive(true);

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (playerHUD != null)
            playerHUD.SetActive(false);

        isInUICamera = true;
    }

    public void SwitchToMainCamera()
    {
        if (!isInUICamera) return;

        uiCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        if (playerHUD != null)
            playerHUD.SetActive(true);

        isInUICamera = false;
    }

    public bool IsInUICamera() => isInUICamera;
}
