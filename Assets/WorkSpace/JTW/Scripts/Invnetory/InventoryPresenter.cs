using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    private bool _isSwitchActivate = false;

    private Vector2 _panelSize;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!IsTrade)
            {
                Manager.Player.Stats.isFarming = false;
            }
            Destroy(this.gameObject);
        }

        if (!_isActivate) return;

        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseItem();
        }

        
    }

    private void LateUpdate()
    {
        // Update 한 사이클이 끝나고 변환해야 MoveInventory가 정상 작동
        if (_isSwitchActivate)
        {
            _isActivate = !_isActivate;
            _isSwitchActivate = false;
            if (_isActivate)
            {
                UpdateItemInfo();
            }
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
        _panelSize = new Vector2(5, 2);
        InitInventory();
    }

    public void SetPanelSize(Vector2 size)
    {
        Destroy(_itemSlotUIs.gameObject);
        _panelSize = size;
        InitInventory();

    }

    public void InitInventory()
    {
        Transform itemSlotsPanel = GetUI("ItemSlotsPanel").transform;

        _itemSlotUIs = Instantiate(_itemSlotsPrefab, itemSlotsPanel).GetComponent<ItemSlotUIs>();
        _itemSlotUIs.SetPanelSize(_panelSize);

        foreach(Slot slot in _inven.SlotList)
        {
            _itemSlotUIs.AddSlotUI(slot);
        }

        _itemSlotUIs.SelectSlotUI(0);
        UpdateItemInfo();
    }

    public bool AddItem(Item item)
    {
        // 겹칠수 있는게 있는지 먼저 탐색
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
                if (!_inventoryForTrade.AddItem(item)) return;
                _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].Slot.RemoveItem();
                UpdateItemInfo();
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
            _inventoryForTrade.Activate(_itemSlotUIs.SelectedSlotIndex);
            return;
        }

        _itemSlotUIs.MoveSelectSlot(movePos);
        UpdateItemInfo();
    }

    private void UpdateItemInfo()
    {
        if (!_isActivate) return;

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

    public void Activate(int index)
    {
        _isSwitchActivate = true;

        index = SetSelectIndex(index);

        _itemSlotUIs.Activate(index);
    }

    public void Deactivate()
    {
        _isSwitchActivate = true;
        GetUI<TextMeshProUGUI>("ItemNameText").text = "";
        GetUI<TextMeshProUGUI>("ItemDescriptionText").text = "";
        _itemSlotUIs.Deactivate();
    }

    private int SetSelectIndex(int index)
    {
        if(_tradeInvenDirection == Vector2.up)
        {
            return index % _itemSlotUIs.LineCount;
        }
        else if (_tradeInvenDirection == Vector2.down)
        {
            return (_itemSlotUIs.SlotUIs.Count + index) - _itemSlotUIs.LineCount;
        }
        else if (_tradeInvenDirection == Vector2.left)
        {
            int selectIndex = index - (_itemSlotUIs.LineCount - 1);

            while(selectIndex > _itemSlotUIs.SlotUIs.Count - 1)
            {
                selectIndex -= _itemSlotUIs.LineCount;
            }

            return selectIndex;
        }
        else if (_tradeInvenDirection == Vector2.right)
        {
            int selectIndex = index + (_itemSlotUIs.LineCount - 1);

            while (selectIndex > _itemSlotUIs.SlotUIs.Count - 1)
            {
                selectIndex -= _itemSlotUIs.LineCount;
            }

            if(selectIndex < 0)
            {
                selectIndex = _itemSlotUIs.SlotUIs.Count - 1;
            }

            return selectIndex;
        }

        return 0;
    }
}
