using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairData : MonoBehaviour, IUsableID
{
    public string ID;
    public string Name;
    public string Description;
    public List<NeedItem> NeedItems = new List<NeedItem>();

    public string GetID()
    {
        return ID;
    }
}
