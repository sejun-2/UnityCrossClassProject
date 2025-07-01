using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour, IDamageable
{
    private PlayerStats Stats => Manager.Player.Stats;

    public void TakeDamage(float amount)
    {
        if (Stats.CurHp.Value <= 0) return;

        Stats.ChangeHp(-amount);

        if(Stats.CurHp.Value <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Manager.Game.LoadSaveData();
    }
}
