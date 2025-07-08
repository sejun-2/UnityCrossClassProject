using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionData : IUsableID
{
    public string ID;
    public Sprite Icon;
    public string Name;
    public string Description;

    public string GetID()
    {
        return ID;
    }
}
