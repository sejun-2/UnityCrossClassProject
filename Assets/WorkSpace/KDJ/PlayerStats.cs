using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStats
{
    public Stat<float> MoveSpeed = new();
    public Stat<int> CurHp = new();
    public Stat<int> MaxHp = new();
    public Stat<int> Hunger = new();
    public Stat<int> Thirst = new();
    public Stat<float> Mentality = new();

    public Stat<PlayerBuffs> Buff = new();

    [JsonIgnore]
    public Stat<Item> Weapon = new();
    [JsonIgnore]
    public Stat<Item> Armor = new();
    public void InitStats()
    {
        CurHp.Value = 100;
        MaxHp.Value = 100;
        Hunger.Value = 100;
        Thirst.Value = 100;
        Mentality.Value = 100;
        MoveSpeed.Value = 5;
        Buff.Value = PlayerBuffs.Nomal;
    }

    public void ChangeHp(float amount)
    {
        if (CurHp.Value < 0) return;

        int changeHp = CurHp.Value + (int)amount;

        if (changeHp < 0)
        {
            CurHp.Value = 0;
        }
        else if (changeHp > MaxHp.Value)
        {
            CurHp.Value = MaxHp.Value;
        }
        else
        {
            CurHp.Value = changeHp;
        }
    }

    public void ChangeMentality(float amount)
    {
        float changeMentality = Mentality.Value + amount;

        if (changeMentality < 0)
        {
            Mentality.Value = 0;
        }
        else if (changeMentality > 100)
        {
            Mentality.Value = 100;
        }
        else
        {
            Mentality.Value = changeMentality;
        }
    }

    public void ChangeHunger(float amount)
    {
        float changeHunger = Hunger.Value + amount;

        if (changeHunger < 0)
        {
            Hunger.Value = 0;
        }
        else if (changeHunger > 100)
        {
            Hunger.Value = 100;
        }
        else
        {
            Hunger.Value = (int)changeHunger;
        }
    }

    public void ChangeThirst(float amount)
    {
        float changeThirst = Thirst.Value + amount;

        if (changeThirst < 0)
        {
            Thirst.Value = 0;
        }
        else if (changeThirst > 100)
        {
            Thirst.Value = 100;
        }
        else
        {
            Thirst.Value = (int)changeThirst;
        }
    }
}
