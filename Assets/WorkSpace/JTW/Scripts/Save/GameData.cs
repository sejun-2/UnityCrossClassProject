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

    public List<ItemSaveData> InvenData = new List<ItemSaveData>();
    public List<ItemSaveData> ItemBoxData = new List<ItemSaveData>();

    public Dictionary<string, bool> IsRepairObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsUsedObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsGetSubStory = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsTalkDialogue = new Dictionary<string, bool>();

    public bool IsInBaseCamp;
    public string SelectedMapName;
}
