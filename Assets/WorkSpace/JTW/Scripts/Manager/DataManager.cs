using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class DataManager : Singleton<DataManager>
{
    private const string _itemDataTableURL = "https://docs.google.com/spreadsheets/d/1BUzeyQqdgSJwcADZ37UqJvqF_LNbXtGZjFV4FOsgHLk/export?format=csv&gid=0";
    private const string _itemNameTableURL = "https://docs.google.com/spreadsheets/d/1IkbcBZ9SV8rxuTtpKtRiC8WMsE8wPh6IyHyhu-JKyvg/export?format=csv&gid=0";
    private const string _craftingTableURL = "https://docs.google.com/spreadsheets/d/1YykeOdfCjzHxt6ixDxeOxsHo-kAuLKVyG1KXayTs-fQ/export?format=csv&gid=0";
    private const string _cookingTableURL = "https://docs.google.com/spreadsheets/d/1nihidDn1fQzNgrbfrwzy0TWjW3SyqWswoYCY5Jo93us/export?format=csv&gid=0";
    private const string _repairTableURL = "https://docs.google.com/spreadsheets/d/1Ll4kkB0pElIZV6ohYWB54NQbv4JPD9yijzrobMuslDM/export?format=csv&gid=0";
    private const string _repairDescriptionTableURL = "https://docs.google.com/spreadsheets/d/1rCK2XdGhGUgGEfJWPJVchXTQTnqh3adUDixuAyX0P1c/export?format=csv&gid=0";
    private const string _ItemDropTableURL = "https://docs.google.com/spreadsheets/d/12WitX8EnRC_vlkSdJI_P3oBDLzqgPZE_0CRtUczMlOM/export?format=csv&gid=0";
    private const string _ItemSearchTableURL = "https://docs.google.com/spreadsheets/d/1acWhQmMpqq8mlE_XdDYcHdSh0Os9_zTbLrw_5cfV_P0/export?format=csv&gid=0";
    private const string _storyDescriptionTableURL = "https://docs.google.com/spreadsheets/d/1jRBAj27nZM4DmqJFzzZAhh8zO3jYZ3TV3wdA2bwf7Fk/export?format=csv&gid=0";
    private const string _playerDialogueTableURL = "https://docs.google.com/spreadsheets/d/1aMfYaGA6_9bvLPwBczY-up_UYgHt4Lg4W-OtRcmY-fU/export?format=csv&gid=0";

    public DataTableParser<Item> ItemData;
    public DataTableParser<CraftingData> CraftingData;
    public DataTableParser<CraftingData> CookingData;
    public DataTableParser<RepairData> RefairData;
    public DataTableParser<ItemSearchData> ItemSearchData;
    public DataTableParser<ItemDropData> ItemDropData;
    public DataTableParser<StoryDescriptionData> StoryDescriptionData;
    public DataTableParser<PlayerDialogueData> PlayerDialogueData;

    private void Awake()
    {
        StartCoroutine(DownloadItemRoutine());
        StartCoroutine(DownloadCraftingRoutine());
        StartCoroutine(DownloadCookingRoutine());
        StartCoroutine(DownloadRepairRoutine());
        StartCoroutine(DownloadSearchRoutine());
        StartCoroutine(DownloadDropRoutine());
        StartCoroutine(DownloadStoryRoutine());
        StartCoroutine(DownloadPlayerDialogueRoutine());
    }

    #region DownloadItem
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

        dataLines = dataLines.Skip(1).ToArray();

        Regex csvSplitRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        for (int i = 0; i < nameLines.Length; i++)
        {
            nameLines[i] = string.Join(",", csvSplitRegex.Split(nameLines[i]).Take(5).ToArray());
        }

        string[] resultLines = new string[dataLines.Length];

        for(int i = 0; i < dataLines.Length; i++)
        {
            resultLines[i] = $"{nameLines[i]},{dataLines[i]}";
        }

        string result = string.Join("\n", resultLines);

        ItemData = new DataTableParser<Item>(words => 
        {
            Item item = ScriptableObject.CreateInstance<Item>();
            int.TryParse(words[0], out item.index);
            item.itemName = words[1].Trim();
            item.description = words[2].Trim();
            string iconName = words[6].Trim();
            string iconPath = $"Assets/Imports/UnityCrossClassProject_Assets/Icons/{words[0]}.png";
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
            float.TryParse(words[19], out item.attackRange);
            item.animationName = words[20];
            return item;
        });

        ItemData.Load(result);
    }
    #endregion

    #region DownloadCrafting
    IEnumerator DownloadCraftingRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_craftingTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        CraftingData = new DataTableParser<CraftingData>(words =>
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
        });

        CraftingData.Load(dataCsv);
    }
    #endregion

    #region DownloadCooking
    IEnumerator DownloadCookingRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_cookingTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        CookingData = new DataTableParser<CraftingData>(words =>
        {
            CraftingData craft = new CraftingData();

            craft.ID = words[0];
            craft.ResultItemID = words[1];

            for (int i = 2; i < 12; i += 2)
            {
                if (string.IsNullOrEmpty(words[i])) break;

                NeedItem needItem;
                needItem.ItemId = words[i];
                needItem.count = int.Parse(words[i + 1]);

                craft.NeedItems.Add(needItem);
            }

            return craft;
        });

        CookingData.Load(dataCsv);
    }
    #endregion

    #region DownloadRepair
    IEnumerator DownloadRepairRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_repairTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        request = UnityWebRequest.Get(_repairDescriptionTableURL);
        yield return request.SendWebRequest();
        string DescriptiondataCsv = request.downloadHandler.text;

        string[] dataLines = dataCsv.Split("\n");
        string[] descriptionLine = DescriptiondataCsv.Split("\n");

        Regex csvSplitRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        for (int i = 0; i < descriptionLine.Length; i++)
        {
            descriptionLine[i] = string.Join(",", csvSplitRegex.Split(descriptionLine[i]).Take(3).ToArray());
        }

        string[] resultLines = new string[dataLines.Length];

        for (int i = 0; i < dataLines.Length; i++)
        {
            resultLines[i] = $"{descriptionLine[i]},{dataLines[i]}";
        }

        string result = string.Join("\n", resultLines);

        RefairData = new DataTableParser<RepairData>(words =>
        {
            RepairData repair = new RepairData();

            repair.ID = words[0];
            repair.Name = words[1];
            repair.Description = words[2];
            Manager.Game.IsRepairObject[words[0]] = false;
            Manager.Game.IsUsedObject[words[0]] = false;

            for (int i = 4; i < 14; i += 2)
            {
                if (string.IsNullOrEmpty(words[i])) break;

                NeedItem needItem;
                needItem.ItemId = words[i];
                needItem.count = int.Parse(words[i + 1]);

                repair.NeedItems.Add(needItem);
            }

            return repair;
        });

        RefairData.Load(result);

        Debug.Log(result);
    }
    #endregion

    #region DownloadDrop
    IEnumerator DownloadDropRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_ItemDropTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        string[] temp = dataCsv.Split("\n");
        temp = temp.Skip(1).ToArray();

        dataCsv = string.Join("\n", temp);

        ItemDropData = new DataTableParser<ItemDropData>(words =>
        {
            ItemDropData drop = new();

            drop.ID = words[0];
            for(int i = 1; i < 6; i++)
            {
                if (float.TryParse(words[i], out float value))
                {
                    drop.ProbabilityList.Add(value);
                }
                else
                {
                    break;
                }
            }

            return drop;
        });

        ItemDropData.Load(dataCsv);
    }
    #endregion

    #region DownloadSearch
    IEnumerator DownloadSearchRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_ItemSearchTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        ItemSearchData = new DataTableParser<ItemSearchData>(words =>
        {
            ItemSearchData search = new();

            search.ID = words[0];

            for(int i = 1; i < 16; i += 3)
            {
                if (string.IsNullOrEmpty(words[i])) break;

                RandomItemData data;
                data.ItemId = words[i];
                data.Probability = float.Parse(words[i + 1]);
                data.Count = int.Parse(words[i + 2]);

                search.RandomItemList.Add(data);
            }

            return search;
        });

        ItemSearchData.Load(dataCsv);
    }
    #endregion

    #region DownloadStory
    IEnumerator DownloadStoryRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_storyDescriptionTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        StoryDescriptionData = new DataTableParser<StoryDescriptionData>(words =>
        {
            StoryDescriptionData story = new StoryDescriptionData();

            story.ID = words[0];
            Manager.Game.IsGetSubStory[words[0]] = false;

            string iconName = words[1].Trim();
            string iconPath = $"Assets/Imports/UnityCrossClassProject_Assets/Icons/{iconName}.png";
            Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
            story.Icon = icon;
            story.Name = words[2];
            story.Description = words[3];
            story.PlayerDialogueID = words[4];

            return story;
        });

        StoryDescriptionData.Load(dataCsv);
    }
    #endregion

    #region DownloadPlayerDialogue
    IEnumerator DownloadPlayerDialogueRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(_playerDialogueTableURL);
        yield return request.SendWebRequest();
        string dataCsv = request.downloadHandler.text;

        PlayerDialogueData = new DataTableParser<PlayerDialogueData>(words =>
        {
            PlayerDialogueData dialogue = new();

            dialogue.ID = words[0];
            dialogue.Dialogue_kr = words[1];
            dialogue.Dialogue_en = words[2];

            return dialogue;
        });

        PlayerDialogueData.Load(dataCsv);
    }
    #endregion
}
