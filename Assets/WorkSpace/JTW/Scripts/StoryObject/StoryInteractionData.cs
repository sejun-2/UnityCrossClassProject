using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInteractionData : IUsableID
{
    public string ID;
    public Sprite Icon;
    public string Description_kr;
    public string Description_en;
    public List<string> PlayerDialogueIndexList = new List<string>();

    public string GetID()
    {
        return ID;
    }
}
