using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToItemBox : MonoBehaviour
{
    [SerializeField] private string _itemId;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Manager.Game.ItemBox.AddItem(Instantiate(Manager.Data.ItemData.Values[_itemId]));
            Debug.Log($"Add {_itemId} item To ItemBox");
        }
    }
}
