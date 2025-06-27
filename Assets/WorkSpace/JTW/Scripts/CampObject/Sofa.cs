using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStats
{
    public void ChangeHp(float amount)
    {
        if (CurHp.Value < 0) return;

        int changeHp = CurHp.Value + (int)amount;

        if(changeHp < 0)
        {
            CurHp.Value = 0;
        }
        else if(changeHp > MaxHp.Value)
        {
            CurHp.Value = MaxHp.Value;
        }
        else
        {
            CurHp.Value = changeHp;
        }
    }
}

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
