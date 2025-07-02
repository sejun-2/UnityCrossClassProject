#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;



public class ItemCSVConnecter
{
    [MenuItem("Tools/Import Items From CSV")]
    public static void ImportItems()
    {
        string csvPath = "Assets/WorkSpace/KBK/CSV/ItemData - Data.csv";
        string[] lines = File.ReadAllLines(csvPath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            Item item = ScriptableObject.CreateInstance<Item>();

            int.TryParse(values[0], out item.index);
            item.itemName = values[1].Trim();
            item.description = values[2].Trim();
            string iconName = values[3].Trim();
            string iconPath = $"Assets/Imports/UnityCrossClassProject_Assets/Icons/{iconName}.png";
            Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
            item.icon = icon;

            int.TryParse(values[4], out item.itemTier);
            item.itemType = (ItemType)int.Parse(values[5]);

            int.TryParse(values[6], out item.attackValue);
            int.TryParse(values[7], out item.defValue);
            if (string.IsNullOrEmpty(values[8]))
            {
                item.durabilityValue = int.Parse(values[8]);
            }
            float.TryParse(values[9], out item.attackSpeed);

            int.TryParse(values[10], out item.hpRecover);
            int.TryParse(values[11], out item.hungerRecover);
            int.TryParse(values[12], out item.thirstRecover);
            int.TryParse(values[13], out item.mentalRecover);

            bool.TryParse(values[14], out item.canStack);
            int.TryParse(values[15], out item.maxStackCount);

            string assetPath = $"Assets/WorkSpace/KBK/Items/{item.index}.asset";
            Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
            AssetDatabase.CreateAsset(item, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif


