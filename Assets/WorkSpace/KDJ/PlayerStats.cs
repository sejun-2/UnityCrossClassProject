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
}
