using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] string ObjectId = "1001";
    public void Interact()
    {
        if (!Manager.Game.IsUsedObject[ObjectId])
        {
            Manager.Player.Stats.ChangeHp(10);
            Manager.Game.IsUsedObject[ObjectId] = true;
        }
        else
        {
            Debug.Log("오늘은 충분히 쉬었어.");
        }
    }
}
