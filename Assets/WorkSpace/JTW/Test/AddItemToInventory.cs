using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToInventory : MonoBehaviour
{
    [SerializeField] private InventoryPresenter _pre;
    [SerializeField] private Item _testItem;
    [SerializeField] private Item.ItemType _type;
    [SerializeField] private string _name;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _pre.SetInventory(new Inventory());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Item item = Instantiate(_testItem);
            item.Type = _type;
            item.Name = _name;
            _pre.AddItem(item);
        }
    }

}
