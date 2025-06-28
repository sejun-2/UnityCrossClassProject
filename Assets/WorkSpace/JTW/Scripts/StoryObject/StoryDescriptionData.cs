using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDescriptionData : IUsableID
{
    public string ID;
    public Sprite Icon;
    public string Name;
    public string Description;
    public string PlayerDialogueID;

    public string GetID()
    {
        return ID;
    }
}
