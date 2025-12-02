using UnityEngine;

public static class DialoguePersistence
{
    public static void MarkSeen(string id)
    {
        PlayerPrefs.SetInt("dialogue_" + id, 1);
    }

    public static bool WasSeen(string id)
    {
        return PlayerPrefs.GetInt("dialogue_" + id, 0) == 1;
    }
}
