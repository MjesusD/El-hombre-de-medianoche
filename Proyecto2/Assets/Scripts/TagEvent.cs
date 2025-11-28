using UnityEngine;
using System.Collections.Generic;

public class TagEventHandler : MonoBehaviour
{
    [Header("Paneles asignables por tag")]
    public List<TagPanelPair> tagPanels;
   

    [System.Serializable]
    public class TagPanelPair
    {
        public string tag;
        public GameObject panel;
    }


    void OnEnable()
    {
        InkDialogueManager.OnTagsReceived += ProcessTags;
    }

    void OnDisable()
    {
        InkDialogueManager.OnTagsReceived -= ProcessTags;
    }

    void ProcessTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            foreach (var pair in tagPanels)
            {
                if (tag == pair.tag && pair.panel != null)
                {
                    pair.panel.SetActive(true);
                    Debug.Log($"[TagEventHandler] Activando panel por TAG: {tag}");
                }
            }
        }
    }
}
