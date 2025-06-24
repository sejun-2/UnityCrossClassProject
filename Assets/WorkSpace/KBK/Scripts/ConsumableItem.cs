using ItemDataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableItem : ItemBase
{
    //체력회복
    public int hpRestore;
    //배고픔회복
    public int hungerRestore;
    //목마름회복
    public int thirstRestore;
    //갯수
    public int itemCount;
    //상태이상회복
    public bool cureStatusEffect;
    //신선도
    public int freshness;
}
