using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : UICanvas<InventoryCanvas>
{
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

    public void ShowCraftingUI()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/CraftingUI");

        Instantiate(prefab, transform);
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

    public void ShowSubStoryUI()
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Inventory/SubStoryUI");

        Instantiate(prefab, transform);
    }
}
