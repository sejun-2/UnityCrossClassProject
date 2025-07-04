using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : UICanvas<InventoryCanvas>
{
    #region Inven,ItemBox,Farming
    public void ShowInven()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/Inventory");
        InventoryPresenter inven = Instantiate(prefab, transform).GetComponent<InventoryPresenter>();

        inven.SetInventory(Manager.Game.Inven);
    }

    public void ShowItemBox()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/ItemBox");
        ItemBoxPresenter itemBox = Instantiate(prefab, transform).GetComponent<ItemBoxPresenter>();

        itemBox.SetItemBoxData(Manager.Game.ItemBox);
    }

    public void ShowTradeItemBox()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/Inventory");
        InventoryPresenter inven = Instantiate(prefab, transform).GetComponent<InventoryPresenter>();

        prefab = Resources.Load<GameObject>($"UI/Inventory/ItemBox");
        ItemBoxPresenter itemBox = Instantiate(prefab, transform).GetComponent<ItemBoxPresenter>();

        inven.SetInventory(Manager.Game.Inven, itemBox, Vector2.right);
        itemBox.SetItemBoxData(Manager.Game.ItemBox, inven, Vector2.left);

        RectTransform invenRt = inven.GetComponent<RectTransform>();
        RectTransform itemBoxRt = itemBox.GetComponent<RectTransform>();

        invenRt.anchoredPosition = new Vector2(-400, 0);
        itemBoxRt.anchoredPosition = new Vector2(400, 0);

        inven.Deactivate();
    }

    public void ShowTradeFarming(Inventory farmingInvenData = null)
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/Inventory");
        InventoryPresenter playerInven = Instantiate(prefab, transform).GetComponent<InventoryPresenter>();

        prefab = Resources.Load<GameObject>($"UI/Inventory/FarmingInventory");
        InventoryPresenter farmingInven = Instantiate(prefab, transform).GetComponent<InventoryPresenter>();

        playerInven.SetInventory(Manager.Game.Inven, farmingInven, Vector2.up);
        farmingInven.SetInventory(farmingInvenData, playerInven, Vector2.down);
        farmingInven.SetPanelSize(new Vector2(5, 1));

        RectTransform invenRt = playerInven.GetComponent<RectTransform>();
        RectTransform itemBoxRt = farmingInven.GetComponent<RectTransform>();

        invenRt.anchoredPosition = new Vector2(-400, -80);
        itemBoxRt.anchoredPosition = new Vector2(-400, 0);

        playerInven.Deactivate();
    }
    #endregion

    #region Crafting,Cooking,Repair
    public void ShowCraftingUI()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/CraftingUI");

        Instantiate(prefab, transform);
    }

    public GameObject ShowCraftResultUI(Item item)
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/CraftResultUI");

        CraftResultUIPresenter pre = Instantiate(prefab, transform).GetComponent<CraftResultUIPresenter>();

        pre.InitData(item);

        return pre.gameObject;
    }

    public void ShowCookingUI()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/CookingUI");

        Instantiate(prefab, transform);
    }

    public void ShowRepairUI(RepairObject repair)
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/RepairUI");

        RepairPresenter pre = Instantiate(prefab, transform).GetComponent<RepairPresenter>();
        pre.InitRepair(repair);
    }
    #endregion

    public void ShowSubStoryUI()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/SubStoryUI");

        Instantiate(prefab, transform);
    }

    public void ShowStoryPopUp(string storyId)
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/StoryPopUp");

        StoryPopUpPresenter pre = Instantiate(prefab, transform).GetComponent<StoryPopUpPresenter>();
        pre.InitStoryPopUp(storyId);
    }

    #region BubbleText
    private GameObject _bubble;
    private Coroutine _bubbleCoroutine;

    public void ShowBubbleText(string dialogueId) => ShowBubbleText(new List<string>() { dialogueId });

    public void ShowBubbleText(List<string> dialogueIdList)
    {
        if (dialogueIdList.Count <= 0) return;
        if (Manager.Game.IsTalkDialogue[dialogueIdList[0]]) return;

        if (_bubbleCoroutine != null)
        {
            StopCoroutine(_bubbleCoroutine);
            Destroy(_bubble);
        }

        _bubbleCoroutine = StartCoroutine(BubbleTextCoroutine(dialogueIdList));
    }

    private IEnumerator BubbleTextCoroutine(List<string> dialogueIdList)
    {
        foreach(string dialogueId in dialogueIdList)
        {
            Manager.Game.IsTalkDialogue[dialogueId] = true;

            string text = Manager.Data.PlayerDialogueData.Values[dialogueId].Dialogue_kr;

            GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/Bubble");

            _bubble = Instantiate(prefab, transform);

            BubbleText bubbleText = _bubble.GetComponent<BubbleText>();
            bubbleText.SetText(text);

            float timer = 5f;

            RectTransform bubbleRt = _bubble.GetComponent<RectTransform>();

            Vector3 offset = new Vector3(0, 2, 0);

            while (timer > 0)
            {
                if (_bubble == null) break;

                timer -= Time.deltaTime;
                Vector3 pos = Manager.Player.Transform.position + offset;

                Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);

                bubbleRt.position = screenPos;

                yield return null;
            }

            if (_bubble != null)
            {
                Destroy(_bubble);
            }
        }

       
    }
    #endregion

    public void ShowMapUI()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/MapUI");

        Instantiate(prefab, transform);
    }

    public void ShowStoryInteractionPopUp(string storyInteractionId)
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/StoryInteractionPopUp");

        StoryInteractionPopUpPresenter pre = Instantiate(prefab, transform).GetComponent<StoryInteractionPopUpPresenter>();

        pre.InitStory(storyInteractionId);
    }

    public GameObject ShowDangerPopUp()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/DangerPopUp");

        return Instantiate(prefab, transform);
    }
}
