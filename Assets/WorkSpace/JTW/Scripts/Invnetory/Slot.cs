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
    public bool IsEmpty => _curItem == null;

    public event Action<Item> OnItemChanged;

    public Slot()
    {
        
    }

    // 테스트용 코드
    public Slot(Item item)
    {
        Debug.Log(GetItem(item));
    }

    public void UseItem()
    {
        _curItem.Use();


        // TODO : 아이템 타입에 따른 변동사항 적용
        _itemCount--;
        if(_itemCount <= 0)
        {
            RemoveItem();
        }
    }

    public bool GetItem(Item item)
    {
        if(IsEmpty || _curItem.Name == item.Name)
        {
            if (IsEmpty)
            {
                _curItem = item;
            }

            OnItemChanged?.Invoke(_curItem);
            _itemCount++;
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
