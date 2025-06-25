using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxData
{
    private List<Slot> _slotList = new List<Slot>();
    public List<Slot> SlotList => _slotList;

    public void AddItem(Item item)
    {
        foreach(Slot slot in _slotList)
        {
            if (slot.AddItem(item))
            {
                return; 
            }
        }

        Slot newSlot = new Slot(int.MaxValue);
        newSlot.AddItem(item);

        _slotList.Add(newSlot);
    }
}
