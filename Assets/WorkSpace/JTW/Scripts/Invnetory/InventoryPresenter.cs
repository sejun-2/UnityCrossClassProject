using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryPresenter : BaseUI, IInventory
{
    [SerializeField] private GameObject _itemSlotsPrefab;
    [SerializeField] private GameObject _slotUIPrefab;

    private Inventory _inven = new();
    private ItemSlotUIs _itemSlotUIs;

    private IInventory _inventoryForTrade;
    private Vector2 _tradeInvenDirection;
    private bool IsTrade => _inventoryForTrade != null;

    private bool _isActivate = true;

    private void Update()
    {
        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseItem();
        }
    }

    public void SetInventory(Inventory inven, IInventory tradeInven = null, Vector2 tradeInvenDirection = default)
    {
        _inven = inven;
        if(tradeInven != null)
        {
            _inventoryForTrade = tradeInven;
            _tradeInvenDirection = tradeInvenDirection;
        }
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
            if(!slot.IsEmpty && slot.CurItem.itemName == item.itemName)
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
        if (IsTrade)
        {
            Item item = _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].Slot.CurItem;

            if(item != null)
            {
                _inventoryForTrade.AddItem(item);
                _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].Slot.RemoveItem();
            }
        }
        else
        {
            _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].UseItem();
        }
    }

    private void MoveInventory()
    {
        if (!_isActivate) return;

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
        if(IsTrade && movePos == _tradeInvenDirection && _itemSlotUIs.CanChangeTrade(movePos))
        {
            Deactivate();
            _inventoryForTrade.Activate();
            return;
        }

        _itemSlotUIs.MoveSelectSlot(movePos);
        UpdateItemInfo();
    }

    private void UpdateItemInfo()
    {
        Item item = _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].GetItemData();

        if (item != null)
        {
            GetUI<TextMeshProUGUI>("ItemNameText").text = item.itemName;
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = item.description;
        }
        else
        {
            GetUI<TextMeshProUGUI>("ItemNameText").text = "";
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = "";
        }
    }

    public void Activate()
    {
        _isActivate = true;
        _itemSlotUIs.Activate();
    }

    public void Deactivate()
    {
        _isActivate = false;
        _itemSlotUIs.Deactivate();
    }
}
