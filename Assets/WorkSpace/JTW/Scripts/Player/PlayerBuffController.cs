using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public partial class PlayerStats
{
    public Stat<PlayerBuffs> Buff = new();
}

public enum PlayerBuffs
{
    Nomal, Full, Fear, Paranoia, Hunger, Thirst, Helplessness
}

public class PlayerBuffController
{
    public float SpeedBuff;
    public float ItemBuff;
    public float VulnerableDebuff;

    private PlayerStats Stats => Manager.Player.Stats;

    public PlayerBuffController()
    {
        Manager.Player.Stats.Buff.OnChanged += ApplyBuffStats;
    }

    public void InitBuff()
    {
        SpeedBuff = 0;
        ItemBuff = 0;
        VulnerableDebuff = 1;
    }

    public void ApplyBuff()
    {
        if(Stats.Weapon.Value == null)
        {
            Stats.Buff.Value = PlayerBuffs.Paranoia;
        }
        else if(Stats.CurHp.Value >= 80 
            && Stats.Hunger.Value >= 80
            && Stats.Thirst.Value >= 80
            && Stats.Mentality.Value >= 80)
        {
            Stats.Buff.Value = PlayerBuffs.Full;
        }
        else if(Stats.Mentality.Value <= 50)
        {
            Stats.Buff.Value = PlayerBuffs.Helplessness;
        }
        else if(Stats.Thirst.Value <= 50)
        {
            Stats.Buff.Value = PlayerBuffs.Thirst;
        }
        else if(Stats.Hunger.Value <= 50)
        {
            Stats.Buff.Value = PlayerBuffs.Hunger;
        }
        else
        {
            Stats.Buff.Value = PlayerBuffs.Nomal;
        }
    }

    public void ApplyBuffStats(PlayerBuffs buff)
    {
        InitBuff();

        switch (buff)
        {
            case PlayerBuffs.Full:
                SpeedBuff = Stats.MoveSpeed.Value * 0.1f;
                ItemBuff = 10;
                break;
            case PlayerBuffs.Fear:
                ItemBuff = -10;
                break;
            case PlayerBuffs.Paranoia:
                SpeedBuff = Stats.MoveSpeed.Value * 0.1f;
                VulnerableDebuff = 2;
                break;
            case PlayerBuffs.Hunger:
                SpeedBuff = -(Stats.MoveSpeed.Value * 0.1f);
                break;
            case PlayerBuffs.Thirst:
                SpeedBuff = -(Stats.MoveSpeed.Value * 0.1f);
                break;
            case PlayerBuffs.Helplessness:
                SpeedBuff = -(Stats.MoveSpeed.Value * 0.1f);
                ItemBuff = -10;
                break;
            case PlayerBuffs.Nomal:
                break;
        }
    }
}
