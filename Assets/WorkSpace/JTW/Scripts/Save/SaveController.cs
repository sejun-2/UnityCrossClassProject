using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController
{
    private string _savePath;

    public void InitPath()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log($"세이브 데이터 경로 : {_savePath}");
    }
    public void SaveGameData()
    {
        Debug.Log($"저장 경로: {_savePath}");

        Debug.Log("Save Game Data!");
        GameData data = new GameData();

        data.stats = Manager.Player.Stats;

        Item weapon = Manager.Player.Stats.Weapon.Value;
        if (weapon != null)
        {
            ItemSaveData itemSaveData;

            itemSaveData.Id = weapon.index.ToString();
            itemSaveData.Durability = weapon.durabilityValue;

            data.WeaponData = itemSaveData;
        }

        Item armor = Manager.Player.Stats.Armor.Value;
        if(armor != null)
        {
            ItemSaveData itemSaveData;

            itemSaveData.Id = armor.index.ToString();
            itemSaveData.Durability = armor.durabilityValue;

            data.ArmorData = itemSaveData;
        }
        

        foreach(Slot slot in Manager.Game.Inven.SlotList)
        {
            if (slot.CurItem == null) continue;

            ItemSaveData itemSaveData;

            itemSaveData.Id = slot.CurItem.index.ToString();
            itemSaveData.Durability = slot.CurItem.durabilityValue;

            if (data.InvenData.ContainsKey(itemSaveData))
            {
                data.InvenData[itemSaveData] += slot.ItemCount;
            }
            else
            {
                data.InvenData.Add(itemSaveData, slot.ItemCount);
            }
                
        }

        foreach(Slot slot in Manager.Game.ItemBox.SlotList)
        {
            if (slot.CurItem == null) continue;

            ItemSaveData itemSaveData;

            itemSaveData.Id = slot.CurItem.index.ToString();
            itemSaveData.Durability = slot.CurItem.durabilityValue;

            if (data.ItemBoxData.ContainsKey(itemSaveData))
            {
                data.ItemBoxData[itemSaveData] += slot.ItemCount;
            }
            else
            {
                data.ItemBoxData.Add(itemSaveData, slot.ItemCount);
            }
        }

        data.IsRepairObject = Manager.Game.IsRepairObject;
        data.IsGetSubStory = Manager.Game.IsGetSubStory;
        data.IsUsedObject = Manager.Game.IsUsedObject;

        data.IsInBaseCamp = Manager.Game.IsInBaseCamp;
        data.SelectedMapName = Manager.Game.SelectedMapName;

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_savePath, json);
    }

    public GameData LoadGameData()
    {
        if (File.Exists(_savePath))
        {
            Debug.Log("Load Game Data!");
            string json = File.ReadAllText(_savePath);
            GameData data = JsonConvert.DeserializeObject<GameData>(json);
            return data;
        }
        else
        {
            Debug.Log("저장된 파일이 없습니다.");
            return null;
        }
    }
}
