using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPresenter : BaseUI
{
    [SerializeField] private ItemSlotUIs _itemSlotUIsPrefab;
    private ItemSlotUIs _needItemSlots;
    private GameObject _needItemPanel;

    private List<NeedItem> _needItemList = new List<NeedItem>();

    private RepairObject _repairObject;

    private bool _canRepair;

    private Dictionary<string, CraftingData> RepairDict => Manager.Data.RefairData.Values;

    private void Start()
    {
        _needItemPanel = GetUI("NeedItemPanel");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && _canRepair)
        {
            Manager.Game.IsRepairObject[_repairObject.ObjectId] = true;
            _repairObject.Repair();

            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Destroy(gameObject);
        }
    }

    public void InitRepair(RepairObject repair)
    {
        _repairObject = repair;

        _needItemList = RepairDict[_repairObject.ObjectId].NeedItems;

        UpdateNeedItemList();
    }

    private void UpdateNeedItemList()
    {
        _needItemSlots = Instantiate(_itemSlotUIsPrefab, _needItemPanel.transform)
            .GetComponent<ItemSlotUIs>();
        _needItemSlots.SetPanelSize(new Vector2(5, 4));
        _needItemSlots.Deactivate();

        _canRepair = true;
        for (int i = 0; i < _needItemList.Count; i++)
        {
            Slot slot = new Slot(int.MaxValue);
            for (int j = 0; j < _needItemList[i].count; j++)
            {
                slot.AddItem(Manager.Data.ItemData.Values[_needItemList[i].ItemId]);
            }

            _needItemSlots.AddSlotUI(slot);

            if (!Manager.Game.ItemBox
                .IsItemExist(_needItemList[i].ItemId, _needItemList[i].count))
            {
                _needItemSlots.SlotUIs[i].SetColor(Color.red);
                _canRepair = false;
            }
        }
    }
}
