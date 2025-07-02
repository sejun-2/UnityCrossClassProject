using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToInven : MonoBehaviour
{

    [SerializeField] private string _itemId;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Manager.Game.Inven.AddItem(Instantiate(Manager.Data.ItemData.Values[_itemId]));
            Debug.Log($"Add {_itemId} item To ItemBox");
        }
    }
}

