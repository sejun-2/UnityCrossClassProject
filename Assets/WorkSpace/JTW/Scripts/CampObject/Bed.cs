using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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
