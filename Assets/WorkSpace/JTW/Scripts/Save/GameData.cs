using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct ItemSaveData
{
    public string Id;
    public int Durability;
}

public class GameData
{
    

    public PlayerStats stats;
    public ItemSaveData WeaponData;
    public ItemSaveData ArmorData;

    public Dictionary<ItemSaveData, int> InvenData = new Dictionary<ItemSaveData, int>();
    public Dictionary<ItemSaveData, int> ItemBoxData = new Dictionary<ItemSaveData, int>();

    public Dictionary<string, bool> IsRepairObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsUsedObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsGetSubStory = new Dictionary<string, bool>();

    public bool IsInBaseCamp;
    public string SelectedMapName;
}
