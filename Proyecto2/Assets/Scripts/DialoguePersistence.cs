using UnityEngine;

public static class DialoguePersistence
{
    public static bool WasSeen(string id)
    {
        return PlayerPrefs.GetInt("DIALOGUE_" + id, 0) == 1;
    }

    public static void MarkAsSeen(string id)
    {
        PlayerPrefs.SetInt("DIALOGUE_" + id, 1);
    }
}
