using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public PlayerStats stats;
    public string WeaponId;
    public string ArmorId;

    public Dictionary<string, int> InvenData = new Dictionary<string, int>();
    public Dictionary<string, int> ItemBoxData = new Dictionary<string, int>();

    public Dictionary<string, bool> IsRepairObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsUsedObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsGetSubStory = new Dictionary<string, bool>();

    public bool IsInBaseCamp;
    public string SelectedMapName;
}
