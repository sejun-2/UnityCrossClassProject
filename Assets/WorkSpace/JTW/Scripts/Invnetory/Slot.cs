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
    private int _maxItemCount = 4;
    public bool IsEmpty => _curItem == null;

    public event Action<Item> OnItemChanged;

    public Slot(int maxItemCount = 4)
    {
        _maxItemCount = maxItemCount;
    }

    public void UseItem()
    {
        _curItem.Use();

        // TODO : 아이템 Type에 따른 변동사항 적용
        _itemCount--;
        OnItemChanged?.Invoke(_curItem);
        if(_itemCount <= 0)
        {
            RemoveItem();
        }
    }

    public bool AddItem(Item item)
    {
        if(IsEmpty || _curItem.Name == item.Name)
        {
            // TODO : Type에 따른 최대 개수 제한 설정
            if (_itemCount >= _maxItemCount) return false;

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

    public void RemoveItem()
    {
        _curItem = null;
        _itemCount = 0;
        OnItemChanged?.Invoke(_curItem);
    }
}
