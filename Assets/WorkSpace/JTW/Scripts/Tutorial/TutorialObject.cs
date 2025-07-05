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
        // �׽�Ʈ�� ��, Awake���� �ϸ� DataManger�� �ٿ�ޱ� ����
        // AddRandomItems�� ����Ǽ� �׳� ��ȣ�ۿ��� �� ������ �ʱ�ȭ�ϰ� ����
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
