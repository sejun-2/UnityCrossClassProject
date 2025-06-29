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
            data.WeaponId = weapon.index.ToString();
        }

        Item armor = Manager.Player.Stats.Armor.Value;
        if(armor != null)
        {
            data.ArmorId = armor.index.ToString();
        }
        

        foreach(Slot slot in Manager.Game.Inven.SlotList)
        {
            if (slot.CurItem == null) continue;

            string slotItemIndex = slot.CurItem.index.ToString();

            if (data.InvenData.ContainsKey(slotItemIndex))
            {
                data.InvenData[slotItemIndex] += slot.ItemCount;
            }
            else
            {
                data.InvenData.Add(slotItemIndex, slot.ItemCount);
            }
                
        }

        foreach(Slot slot in Manager.Game.ItemBox.SlotList)
        {
            if (slot.CurItem == null) continue;

            string slotItemIndex = slot.CurItem.index.ToString();

            if (data.ItemBoxData.ContainsKey(slotItemIndex))
            {
                data.ItemBoxData[slotItemIndex] += slot.ItemCount;
            }
            else
            {
                data.ItemBoxData.Add(slotItemIndex, slot.ItemCount);
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
