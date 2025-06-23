using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Slot
{
    private Item _curItem;
    public Item CurItem => _curItem;
    private int _itemCount;
    public int ItemCount => _itemCount;
    public bool IsEmpty => _curItem == null;

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
        _itemCount--;
        if(_itemCount <= 0)
        {
            RemoveItem();
        }
    }

    public bool GetItem(Item item)
    {
        if (IsEmpty)
        {
            _curItem = item;
            _itemCount++;
            return true;
        }
        else if(_curItem.Name == item.Name)
        {
            _itemCount++;
            return true;
        }

        return false;
    }

    public void RemoveItem()
    {
        _curItem = null;
        _itemCount = 0;
    }
}
