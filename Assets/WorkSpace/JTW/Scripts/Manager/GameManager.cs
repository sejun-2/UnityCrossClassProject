using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Inventory Inven;
    public ItemBoxData ItemBox;

    public Dictionary<string, bool> IsRepairObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsUsedObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsGetSubStory = new Dictionary<string, bool>();

    public bool IsInBaseCamp = true;
    public string SelectedMapName = "";

    public float BarricadeHp = 100;

    private SaveController _saveContoroller = new SaveController();

    // 하루가 마무리 될 때, 즉 베이스캠프로 돌아올 때 발생
    public event Action OnDayCompleted;

    private void Awake()
    {
        Inven = new Inventory();
        ItemBox = new ItemBoxData();

        _saveContoroller.InitPath();
    }

    public void GameStart()
    {
        GameData data = _saveContoroller.LoadGameData();

        if (data == null)
        {
            ChangeScene("Tutorial");
        }
        else
        {
            Manager.Player.Stats = data.stats;
            Manager.Player.Stats.Weapon = new Stat<Item>();
            Manager.Player.Stats.Armor = new Stat<Item>();

            if (!string.IsNullOrEmpty(data.WeaponData.Id))
            {
                Manager.Player.Stats.Weapon.Value = Instantiate(Manager.Data.ItemData.Values[data.WeaponData.Id]);
                Manager.Player.Stats.Weapon.Value.durabilityValue = data.WeaponData.Durability;
            }

            if (!string.IsNullOrEmpty(data.ArmorData.Id))
            {
                Manager.Player.Stats.Armor.Value = Instantiate(Manager.Data.ItemData.Values[data.ArmorData.Id]);
                Manager.Player.Stats.Weapon.Value.durabilityValue = data.ArmorData.Durability;
            }

            foreach (KeyValuePair<ItemSaveData, int> value in data.InvenData)
            {
                Item item = Instantiate(Manager.Data.ItemData.Values[value.Key.Id]);
                item.durabilityValue = value.Key.Durability;

                for(int i = 0; i < value.Value; i++)
                {
                    Inven.AddItem(item);
                }
            }

            foreach(KeyValuePair<ItemSaveData, int> value in data.ItemBoxData)
            {
                Item item = Instantiate(Manager.Data.ItemData.Values[value.Key.Id]);
                item.durabilityValue = value.Key.Durability;

                for (int i = 0; i < value.Value; i++)
                {
                    ItemBox.AddItem(item);
                }
            }

            IsRepairObject = data.IsRepairObject;
            IsUsedObject = data.IsUsedObject;
            IsGetSubStory = data.IsGetSubStory;

            IsInBaseCamp = data.IsInBaseCamp;
            SelectedMapName = data.SelectedMapName;

            if (IsInBaseCamp)
            {
                ChangeScene("BaseCamp");
            }
            else
            {
                ChangeScene(SelectedMapName);
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        if(sceneName == "BaseCamp")
        {
            DayComplete();
        }

        SceneManager.LoadScene(sceneName);
    }

    public void DayComplete()
    {
        foreach(string key in IsUsedObject.Keys)
        {
            IsUsedObject[key] = false;
        }

        IsInBaseCamp = true;

        OnDayCompleted?.Invoke();

        _saveContoroller.SaveGameData();
    }
}
