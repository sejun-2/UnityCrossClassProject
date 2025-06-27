using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public partial class PlayerStats
{
    public Stat<float> Mentality = new();

    public void ChangeMentality(float amount)
    {
        float changeMentality = Mentality.Value + amount;

        if(changeMentality < 0)
        {
            Mentality.Value = 0;
        } else if(changeMentality > 100)
        {
            Mentality.Value = 100;
        }
        else
        {
            Mentality.Value = changeMentality;
        }
    }
}

public class Bed : MonoBehaviour
{
    [SerializeField] string ObjectId = "1002";
    public void Interact()
    {
        if (!Manager.Game.IsUsedObject[ObjectId])
        {
            Manager.Player.Stats.ChangeMentality(10);
            Manager.Game.IsUsedObject[ObjectId] = true;
        }
        else
        {
            Debug.Log("오늘은 충분히 쉬었어.");
        }
    }
}
