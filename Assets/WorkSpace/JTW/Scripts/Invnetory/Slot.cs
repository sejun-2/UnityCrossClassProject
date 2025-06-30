using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    private Item _curItem;
    public Item CurItem => _curItem;
    private int _itemCount;
    public int ItemCount => _itemCount;
    private int _CustomMaxItemCount;
    public bool IsEmpty => _curItem == null;

    public event Action<Item> OnItemChanged;

    public bool _isCustomStack;

    public Slot(int CustomItemCount = 0)
    {
        if(CustomItemCount != 0)
        {
            _CustomMaxItemCount = CustomItemCount;
            _isCustomStack = true;
        }
    }

    public void UseItem()
    {
        if (_curItem.Use())
        {
            RemoveItem();
        }
    }

    public bool AddItem(Item item)
    {
        if(IsEmpty || _curItem.itemName == item.itemName)
        {
            if (!_isCustomStack && _itemCount >= item.maxStackCount) return false;

            if (_isCustomStack && _itemCount >= _CustomMaxItemCount) return false;

            if (!IsEmpty && (_curItem.itemType == ItemType.Weapon || _curItem.itemType == ItemType.Armor)) return false;

            if (IsEmpty)
            {
                _curItem = item;
            }

            _itemCount++;
            OnItemChanged?.Invoke(_curItem);
            return true;
        }

        return false;
    }

    public void DeleteItem()
    {
        _curItem.ClearEvent();
        _curItem = null;
        _itemCount = 0;
        OnItemChanged?.Invoke(_curItem);
    }

    public void RemoveItem()
    {
        _itemCount--;
        OnItemChanged?.Invoke(_curItem);
        if (_itemCount <= 0)
        {
            DeleteItem();
        }
    }
}
