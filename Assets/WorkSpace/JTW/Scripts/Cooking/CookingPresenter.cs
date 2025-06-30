using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPresenter : BaseUI
{
    [SerializeField] private ItemSlotUIs _itemSlotUIsPrefab;
    private ItemSlotUIs _resultItemSlots;
    private ItemSlotUIs _needItemSlots;

    private GameObject _resultItemPanel;
    private GameObject _needItemPanel;

    private List<List<NeedItem>> _needItemList = new List<List<NeedItem>>();

    private bool _canCraft;

    private Dictionary<string, CraftingData> CraftDict => Manager.Data.CookingData.Values;

    private void Start()
    {
        _resultItemPanel = GetUI("ResultItemPanel");
        _needItemPanel = GetUI("NeedItemPanel");

        InitCrafting();
    }

    private void Update()
    {
        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z) && _canCraft)
        {
            Item item = _resultItemSlots.SlotUIs[_resultItemSlots.SelectedSlotIndex].Slot.CurItem;
            Debug.Log($"{item.name} 아이템 제작");

            Manager.Game.ItemBox.AddItem(item);

            foreach (NeedItem need in _needItemList[_resultItemSlots.SelectedSlotIndex])
            {
                for (int i = 0; i < need.count; i++)
                {
                    Manager.Game.ItemBox.RemoveItem(need.ItemId);
                }
            }

            UpdateNeedItemList(_resultItemSlots.SelectedSlotIndex);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Destroy(gameObject);
        }
    }

    public void InitCrafting()
    {
        _resultItemSlots = Instantiate(_itemSlotUIsPrefab, _resultItemPanel.transform)
            .GetComponent<ItemSlotUIs>();
        _resultItemSlots.SetPanelSize(new Vector2(5, 4));

        foreach (CraftingData craft in CraftDict.Values)
        {
            _needItemList.Add(craft.NeedItems);

            Slot slot = new Slot(1);
            slot.AddItem(Manager.Data.ItemData.Values[craft.ResultItemID]);

            _resultItemSlots.AddSlotUI(slot);
        }

        _resultItemSlots.SelectSlotUI(0);
        UpdateNeedItemList(_resultItemSlots.SelectedSlotIndex);
    }

    private void MoveInventory()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _resultItemSlots.MoveSelectSlot(Vector2.right);
            UpdateNeedItemList(_resultItemSlots.SelectedSlotIndex);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _resultItemSlots.MoveSelectSlot(Vector2.left);
            UpdateNeedItemList(_resultItemSlots.SelectedSlotIndex);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _resultItemSlots.MoveSelectSlot(Vector2.up);
            UpdateNeedItemList(_resultItemSlots.SelectedSlotIndex);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _resultItemSlots.MoveSelectSlot(Vector2.down);
            UpdateNeedItemList(_resultItemSlots.SelectedSlotIndex);
        }

    }

    private void UpdateNeedItemList(int index)
    {
        if (_needItemSlots != null)
        {
            Destroy(_needItemSlots.gameObject);
        }

        _needItemSlots = Instantiate(_itemSlotUIsPrefab, _needItemPanel.transform)
            .GetComponent<ItemSlotUIs>();
        _needItemSlots.SetPanelSize(new Vector2(5, 4));
        _needItemSlots.Deactivate();

        _canCraft = true;
        for (int i = 0; i < _needItemList[index].Count; i++)
        {
            Slot slot = new Slot(int.MaxValue);
            for (int j = 0; j < _needItemList[index][i].count; j++)
            {
                slot.AddItem(Manager.Data.ItemData.Values[_needItemList[index][i].ItemId]);
            }

            _needItemSlots.AddSlotUI(slot);

            if (!Manager.Game.ItemBox
                .IsItemExist(_needItemList[index][i].ItemId, _needItemList[index][i].count))
            {
                _needItemSlots.SlotUIs[i].SetColor(Color.red);
                _canCraft = false;
            }
        }
    }
}
