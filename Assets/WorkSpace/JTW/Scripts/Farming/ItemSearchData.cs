using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RandomItemData
{
    public string ItemId;
    public float Probability;
    public int Count;
}

public class ItemSearchData : IUsableID
{
    public string ID;
    public List<RandomItemData> RandomItemList = new List<RandomItemData>();

    public string GetID()
    {
        return ID;
    }
}
