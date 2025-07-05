using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryData : IUsableID
{
    public string Id;
    public string Name;
    public string Description;
    public List<string> PlayerDialogueIndexList;

    public string GetID()
    {
        return Id;
    }
}
