using UnityEngine;

public class ResetDialoguesAlways : MonoBehaviour
{
    void Awake()
    {
        // Borrar todos los diálogos guardados
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Todos los diálogos reiniciados (Editor y Build).");
    }
}
