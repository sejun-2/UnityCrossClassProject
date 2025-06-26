using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NeedItem
{
    public string ItemId;
    public int count;
}

public class CraftingData : IUsableID
{
    public string ID;
    public string ResultItemID;
    public List<NeedItem> NeedItems = new List<NeedItem>();

    public string GetID()
    {
        return ID;
    }
}
