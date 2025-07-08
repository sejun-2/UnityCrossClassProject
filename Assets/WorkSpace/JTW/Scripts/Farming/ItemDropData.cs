using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropData : IUsableID
{
    public string ID;
    public List<float> ProbabilityList = new List<float>();

    public string GetID()
    {
        return ID;
    }
}
