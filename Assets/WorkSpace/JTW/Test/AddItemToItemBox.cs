using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToItemBox : MonoBehaviour
{
    [SerializeField] private ItemBoxPresenter _pre;
    [SerializeField] private Item _testItemPrefab;
    [SerializeField] private Item.ItemType _type;
    [SerializeField] private string _name;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Item item = Instantiate(_testItemPrefab);
            item.Type = _type;
            item.Name = _name;

            _pre.AddItem(item);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            _pre.SetItemBoxData(new ItemBoxData());
        }
    }
}
