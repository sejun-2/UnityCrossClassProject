using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class DataManager : Singleton<DataManager>
{
    // TODO : URL 적용
    private const string _itemDataTableURL = "/export?format=csv&gid=1551223081";
    private const string _itemNameTableURL = "/export?format=csv&gid=1551223081";
    private const string _craftingTableURL = "https://docs.google.com/spreadsheets/d/1YykeOdfCjzHxt6ixDxeOxsHo-kAuLKVyG1KXayTs-fQ/export?format=csv&gid=0";

    // Item에 IUsableID 추가
    public DataTableParser<ItemTest> ItemData;
    public DataTableParser<CraftingData> CraftingData;

    private void Awake()
    {
        // TODO : URL 확정 되고 나서 진행
        // StartCoroutine(DownloaditemRoutine());
    }

    IEnumerator DownloadItemRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_itemDataTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        request = UnityWebRequest.Get(_itemNameTableURL);
        yield return request.SendWebRequest();
        string itemCsv = request.downloadHandler.text;

        string[] dataLines = dataCsv.Split("\n");
        string[] nameLines = itemCsv.Split("\n");


        string[] resultLines = new string[dataLines.Length];

        for(int i = 0; i < dataLines.Length; i++)
        {
            resultLines[i] = $"{nameLines[i]},{dataLines[i]}";
        }

        string result = string.Join("\n", resultLines);

        ItemData.Parse = words =>
        {
            Item item = ScriptableObject.CreateInstance<Item>();
            int.TryParse(words[0], out item.index);
            item.itemName = words[1].Trim();
            item.description = words[2].Trim();
            string iconName = words[6].Trim();
            string iconPath = $"Assets/Imports/UnityCrossClassProject_Assets/Icons/{iconName}.png";
            Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
            item.icon = icon;

            int.TryParse(words[7], out item.itemTier);
            item.itemType = (ItemType)int.Parse(words[8]);

            int.TryParse(words[9], out item.attackValue);
            int.TryParse(words[10], out item.defValue);
            int.TryParse(words[11], out item.durabilityValue);
            float.TryParse(words[12], out item.attackSpeed);

            int.TryParse(words[13], out item.hpRecover);
            int.TryParse(words[14], out item.hungerRecover);
            int.TryParse(words[15], out item.thirstRecover);
            int.TryParse(words[16], out item.mentalRecover);

            bool.TryParse(words[17], out item.canStack);
            int.TryParse(words[18], out item.maxStackCount);
            //return item;
            return new ItemTest();
        };

        ItemData.Load(result);
    }

    IEnumerator DownloadCraftingRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_craftingTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        CraftingData.Parse = words =>
        {
            CraftingData craft = new CraftingData();

            craft.ID = words[0];
            craft.ResultItemID = words[1];

            for(int i = 2; i < 12; i += 2)
            {
                if (string.IsNullOrEmpty(words[i])) break;

                NeedItem needItem;
                needItem.ItemId = words[i];
                needItem.count = int.Parse(words[i + 1]);

                craft.NeedItems.Add(needItem);
            }

            return craft;
        };

        CraftingData.Load(dataCsv);
    }
}
