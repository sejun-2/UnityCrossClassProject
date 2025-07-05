using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ItemCount
{
    public string ItemId;
    public int count;
}

public class TutorialObject : MonoBehaviour, IInteractable
{
    [field: SerializeField] private List<ItemCount> _itemList = new List<ItemCount>();

    private Inventory _farmingInven;

    private bool _isInit;

    private void Awake()
    {
        _farmingInven = new Inventory(new Vector2(5, 1));
    }

    public void Interact()
    {
        // 테스트할 때, Awake에서 하면 DataManger가 다운받기 전에
        // AddRandomItems가 실행되서 그냥 상호작용할 때 아이템 초기화하게 설정
        if (!_isInit)
        {
            AddItems();
            _isInit = true;
        }

        Manager.UI.Inven.ShowTradeFarming(_farmingInven);
        Manager.Player.Stats.isFarming = true;
    }

    private void AddItems()
    {
        foreach (ItemCount item in _itemList)
        {
            for(int i = 0; i < item.count; i++)
            {
                _farmingInven.AddItem(Instantiate(Manager.Data.ItemData.Values[item.ItemId]));
            }
        }
    }

}
