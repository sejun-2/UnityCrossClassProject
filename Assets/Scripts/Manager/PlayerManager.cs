using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerStats _stats = new();
    public PlayerStats Stats 
    { 
        get => _stats; 
        set
        {
            _stats = value;
            _stats.Buff.OnChanged += _buffController.ApplyBuffStats;
        }
    }

    // 현재 생성된 Player 오브젝트의 Transform
    public Transform Transform;

    private PlayerBuffController _buffController;
    public PlayerBuffController BuffStats;

    private void Awake()
    {
        Stats.InitStats();
        _buffController = new PlayerBuffController();
        _buffController.InitBuff();
    }
}
