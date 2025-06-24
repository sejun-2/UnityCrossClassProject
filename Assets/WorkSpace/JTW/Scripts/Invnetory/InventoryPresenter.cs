using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryPresenter : BaseUI
{
    [SerializeField] private GameObject _itemSlotsPrefab;
    [SerializeField] private GameObject _slotUIPrefab;

    private Inventory _inven = new();
    private ItemSlotUIs _itemSlotUIs;

    private void Update()
    {
        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseItem();
        }
    }

    public void SetInventory(Inventory inven)
    {
        _inven = inven;
        InitInventory();
    }

    public void InitInventory()
    {
        Transform itemSlotsPanel = GetUI("ItemSlotsPanel").transform;

        _itemSlotUIs = Instantiate(_itemSlotsPrefab, itemSlotsPanel).GetComponent<ItemSlotUIs>();

        foreach(Slot slot in _inven.SlotList)
        {
            _itemSlotUIs.AddSlotUI(slot);
        }

        _itemSlotUIs.SelectSlotUI(0);
        UpdateItemInfo();
    }

    public bool AddItem(Item item)
    {
        // °ãÄ¥¼ö ÀÖ´Â°Ô ÀÖ´ÂÁö ¸ÕÀú Å½»ö
        foreach(Slot slot in _inven.SlotList)
        {
            if(!slot.IsEmpty && slot.CurItem.Name == item.Name)
            {
                if (slot.AddItem(item))
                {
                    UpdateItemInfo();
                    return true;
                }
            }
        }

        foreach (Slot slot in _inven.SlotList)
        {
            if (slot.AddItem(item))
            {
                UpdateItemInfo();
                return true;
            }
        }

        return false;
    }

    private void UseItem()
    {
        _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].UseItem();
    }

    private void MoveInventory()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelectSlot(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelectSlot(Vector2.left);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelectSlot(Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelectSlot(Vector2.down);
        }
    }

    private void ChangeSelectSlot(Vector2 movePos)
    {
        _itemSlotUIs.MoveSelectSlot(movePos);
        UpdateItemInfo();
    }

    private void UpdateItemInfo()
    {
        Item item = _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].GetItemData();

        if (item != null)
        {
            GetUI<TextMeshProUGUI>("ItemNameText").text = item.Name;
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = item.Description;
        }
        else
        {
            GetUI<TextMeshProUGUI>("ItemNameText").text = "";
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = "";
        }
    }
}
