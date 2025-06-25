using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemDataToGameManager : MonoBehaviour
{
    [SerializeField] Item _testItemPrefab;
    [SerializeField] Item.ItemType _type;
    [SerializeField] string _name;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Item item = ItemSetting();

            Manager.Game.Inven.AddItem(item);
            Debug.Log($"Inven add {item.Name}");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Item item = ItemSetting();

            Manager.Game.ItemBox.AddItem(item);
            Debug.Log($"ItemBox add {item.Name}");
        }
    }

    private Item ItemSetting()
    {
        Item item = Instantiate(_testItemPrefab);
        item.Type = _type;
        item.Name = _name;

        return item;
    }
}
