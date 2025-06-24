using ItemDataManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemCSVConnecter
{
    //����Ƽ���� Tools�޴��߰� > .asset ������ ���� ���������
    [MenuItem("Tools/Import Items From CSV")]
    public static void ImportItems()
    {
        string path = "Assets/WorkSpace/KBK/CSV/ItemData - Data.csv";
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            string name = values[0].Trim();

            //ItemBase.ItemType itemType = (ItemBase.ItemType)System.Enum.Parse(typeof(ItemBase.ItemType), values[1].Trim());

            string rawItemType = values[1].Trim().Replace("\r", "").Replace("\n", "");
            ItemBase.ItemType itemType;
            if (!Enum.TryParse(rawItemType, true, out itemType))
            {
                Debug.LogError($"ItemType �Ľ� ����: '{rawItemType}' > Food�� ó����");
                itemType = ItemBase.ItemType.Food; // fallback
            }
            else
            {
                Debug.Log($"ItemType �Ľ� ����: {rawItemType} > {itemType}");
            }

            string classType = values[2].Trim();

            string iconName = values[3].Trim();
            Sprite icon = Resources.Load<Sprite>("Icons/" + iconName);

            Debug.Log($"[Ŭ���� Ÿ�� Ȯ��] '{classType}'");

            if (classType == "Consumable")
            {
                Debug.Log("Consumable �б� ����");

                ConsumableItem item = ScriptableObject.CreateInstance<ConsumableItem>();
                item.itemName = name;
                item.sprite = icon;
                item.Description = "";

                int.TryParse(values[4], out item.hpRestore);
                int.TryParse(values[5], out item.hungerRestore);
                int.TryParse(values[6], out item.thirstRestore);

                string assetPath = $"Assets/WorkSpace/KBK/Items/Consumable/{name}.asset";
                SaveAsset(item, assetPath);
            }
            else if (classType == "Equipment")
            {
                EquipmentItem item = ScriptableObject.CreateInstance<EquipmentItem>();
                item.itemName = name;
                item.sprite = icon;

                int.TryParse(values[7], out item.addAttackPower);
                int.TryParse(values[8], out item.addDefensePower);
                int.TryParse(values[9], out item.durability);

                string assetPath = $"Assets/WorkSpace/KBK/Items/Equipment/{name}.asset";
                SaveAsset(item, assetPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log(".asset ���� �Ϸ�!");
    }

    static void SaveAsset(UnityEngine.Object obj, string path)
    {
        string folder = Path.GetDirectoryName(path);
        Debug.Log($"[SAVE] ����: {folder}");
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
            Debug.Log($"[SAVE] ���� ���� �õ�: {folder}");
        }

        Debug.Log($"[SAVE] Asset ����: {path}");
        AssetDatabase.CreateAsset(obj, path);
    }
}
