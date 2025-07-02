using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairPresenter : BaseUI
{
    [SerializeField] private ItemSlotUIs _itemSlotUIsPrefab;

    private ItemSlotUIs _needItemSlots;
    private GameObject _needItemPanel;
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _descriptionText;

    private GameObject _barricadePanel;
    private Slider _barricadeSlider;


    private List<NeedItem> _needItemList = new List<NeedItem>();

    private RepairObject _repairObject;

    private bool _canRepair;

    private Dictionary<string, RepairData> RepairDict => Manager.Data.RefairData.Values;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && _canRepair)
        {

            foreach (NeedItem need in _needItemList)
            {
                for (int i = 0; i < need.count; i++)
                {
                    Manager.Game.ItemBox.RemoveItem(need.ItemId);
                }
            }

            if (_repairObject.ObjectId == "4006")
            {
                float changeHp = Manager.Game.BarricadeHp + 30;

                Manager.Game.BarricadeHp = Mathf.Min(100, changeHp);
            }
            else
            {
                Manager.Game.IsRepairObject[_repairObject.ObjectId] = true;
                _repairObject.Repair();
            }

            Destroy(gameObject);
            Manager.Player.Stats.isFarming = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Player.Stats.isFarming = false;
            Destroy(gameObject);
        }
    }

    public void InitRepair(RepairObject repair)
    {
        InitObjects();

        _repairObject = repair;

        _needItemList = RepairDict[_repairObject.ObjectId].NeedItems;

        if (repair.ObjectId == "4006")
        {
            _barricadePanel.SetActive(true);
            _barricadeSlider.value = Manager.Game.BarricadeHp / 100;
        }

        UpdateNeedItemList();
    }
    private void InitObjects()
    {
        _needItemPanel = GetUI("NeedItemPanel");
        _nameText = GetUI<TextMeshProUGUI>("NameText");
        _descriptionText = GetUI<TextMeshProUGUI>("DescriptionText");
        _barricadePanel = GetUI("BarricadePanel");
        _barricadeSlider = GetUI<Slider>("BarricadeSlider");
    }

    private void UpdateNeedItemList()
    {
        _needItemSlots = Instantiate(_itemSlotUIsPrefab, _needItemPanel.transform)
            .GetComponent<ItemSlotUIs>();
        _needItemSlots.SetPanelSize(new Vector2(5, 1));
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

        _nameText.text = RepairDict[_repairObject.ObjectId].Name;
        _descriptionText.text = RepairDict[_repairObject.ObjectId].Description;
    }
}
