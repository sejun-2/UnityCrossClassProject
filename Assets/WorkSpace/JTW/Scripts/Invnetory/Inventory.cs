using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    private Slot[,] _slotList = new Slot[2,5];
    public Slot[,] SlotList => _slotList;

    public Inventory(Vector2 size = default)
    {
        if(size != default)
        {
            _slotList = new Slot[(int)size.x, (int)size.y];
        }

        for(int i = 0; i < _slotList.GetLength(0); i++)
        {
            for(int j = 0; j < _slotList.GetLength(1); j++)
            {
                _slotList[i, j] = new Slot();
            }
        }
    }

    public void AddItem(Item item)
    {
        foreach(Slot slot in _slotList)
        {
            if(slot.CurItem != null && item.itemName == slot.CurItem.itemName)
            {
                if (slot.AddItem(item)) return;
            }
        }

        foreach (Slot slot in _slotList)
        {
            if (slot.AddItem(item)) return;
        }
    }
}