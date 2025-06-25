using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    public bool AddItem(Item item);

    public void Activate();
}
