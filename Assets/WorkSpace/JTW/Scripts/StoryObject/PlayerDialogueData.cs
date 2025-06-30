using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueData : IUsableID
{
    public string ID;
    public string Dialogue_kr;
    public string Dialogue_en;

    public string GetID()
    {
        return ID;
    }
}
