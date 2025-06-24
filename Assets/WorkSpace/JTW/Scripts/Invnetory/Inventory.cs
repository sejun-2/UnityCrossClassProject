using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    private Slot[,] _slotList = new Slot[4,4];
    public Slot[,] SlotList => _slotList;

    public Inventory()
    {
        for(int i = 0; i < _slotList.GetLength(0); i++)
        {
            for(int j = 0; j < _slotList.GetLength(1); j++)
            {
                _slotList[i, j] = new Slot();
            }
        }
    }
}