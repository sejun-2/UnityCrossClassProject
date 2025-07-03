using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmingObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string _dropDataId;
    [SerializeField] private string _searchDataId;

    private Inventory _farmingInven;

    private bool _isInit;

    private void Awake()
    {
        _farmingInven = new Inventory(new Vector2(5,1));
    }

    public void Interact()
    {
        // 테스트할 때, Awake에서 하면 DataManger가 다운받기 전에
        // AddRandomItems가 실행되서 그냥 상호작용할 때 아이템 초기화하게 설정
        if (!_isInit)
        {
            AddRandomItems();
            _isInit = true;
        }

        Manager.UI.Inven.ShowTradeFarming(_farmingInven);
        Manager.Player.Stats.isFarming = true;
    }

    private void AddRandomItems()
    {
        foreach(float probability in Manager.Data.ItemDropData.Values[_dropDataId].ProbabilityList)
        {
            if (!IsDrop(probability)) break;

            AddRandomItem();
        }
    }

    private bool IsDrop(float probability)
    {
        if (probability <= 0) return false;

        float randomValue = Random.value;

        if(probability < 1)
        {
            probability += Manager.Player.BuffStats.ItemBuff;
        }

        return probability >= randomValue;
    }

    private void AddRandomItem()
    {
        List<RandomItemData> ItemList = Manager.Data.ItemSearchData.Values[_searchDataId].RandomItemList;

        float randomValue = Random.value;
        float sum = 0;
        foreach (RandomItemData data in ItemList)
        {
            sum += data.Probability;
            if (randomValue > sum) continue;

            Item item = Instantiate(Manager.Data.ItemData.Values[data.ItemId]);
            for(int i = 0; i < data.Count; i++)
            {
                _farmingInven.AddItem(item);
            }

            return;
        }

        // 혹시 위의 코드가 안될 때를 위한 보험
        Item itemLast = Instantiate(Manager.Data.ItemData.Values[ItemList.Last().ItemId]);
        for (int i = 0; i < ItemList.Last().Count; i++)
        {
            _farmingInven.AddItem(itemLast);
        }
    }
}
