using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingPresenter : BaseUI
{
    [field: SerializeField] private List<Sprite> _categorySprites;
    [SerializeField] private ItemSlotUIs _itemSlotUIsPrefab;
    private ItemSlotUIs _categorySlots;
    private List<ItemSlotUIs> _resultItemSlotsList = new();
    private ItemSlotUIs _needItemSlots;

    private GameObject _categoryPanel;
    private GameObject _resultItemPanel;
    private GameObject _needItemPanel;

    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _descriptionText;

    private List<List<List<NeedItem>>> _needItemList = new List<List<List<NeedItem>>>();

    private bool _canCraft;
    private bool _isInResultItems;

    private ItemSlotUIs _selectedResultItemSlots;

    private Dictionary<string, CraftingData> CraftDict => Manager.Data.CraftingData.Values;

    private void Start()
    {
        _categoryPanel = GetUI("CategoryPanel");
        _resultItemPanel = GetUI("ResultItemPanel");
        _needItemPanel = GetUI("NeedItemPanel");
        _nameText = GetUI<TextMeshProUGUI>("NameText");
        _descriptionText = GetUI<TextMeshProUGUI>("DescriptionText");

        InitCrafting();
    }

    private void Update()
    {
        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z) && _canCraft && _isInResultItems)
        {
            Item item = _selectedResultItemSlots.SlotUIs[_selectedResultItemSlots.SelectedSlotIndex].Slot.CurItem;
            Debug.Log($"{item.name} 아이템 제작");

            Manager.Game.ItemBox.AddItem(item);

            foreach(NeedItem need in _needItemList[_categorySlots.SelectedSlotIndex][_selectedResultItemSlots.SelectedSlotIndex])
            {
                for(int i = 0; i < need.count; i++)
                {
                    Manager.Game.ItemBox.RemoveItem(need.ItemId);
                }
            }

            UpdateNeedItemList(_selectedResultItemSlots.SelectedSlotIndex);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Destroy(gameObject);
        }
    }

    public void InitCrafting()
    {
        _categorySlots = Instantiate(_itemSlotUIsPrefab, _categoryPanel.transform).GetComponent<ItemSlotUIs>();
        _categorySlots.SetPanelSize(new Vector2(5, 1));

        for (int i = 0; i < _categorySprites.Count; i++)
        {
            _categorySlots.AddSlotUI(maxItemCount: 0);
            _categorySlots.SlotUIs[i].ItemImage.sprite = _categorySprites[i];
        }
        _categorySlots.SelectSlotUI(0);

        for(int i = 0; i < _categorySprites.Count; i++)
        {
            _resultItemSlotsList.Add(Instantiate(_itemSlotUIsPrefab, _resultItemPanel.transform)
                .GetComponent<ItemSlotUIs>());
            _resultItemSlotsList[i].SetPanelSize(new Vector2(5, 3));
            _resultItemSlotsList[i].gameObject.SetActive(false);
            _needItemList.Add(new List<List<NeedItem>>());
        }

        _resultItemSlotsList[0].gameObject.SetActive(true);
        _selectedResultItemSlots = _resultItemSlotsList[0];

        foreach (CraftingData craft in CraftDict.Values)
        {

            Slot slot = new Slot(1);

            Item item = Manager.Data.ItemData.Values[craft.ResultItemID];

            _needItemList[item.itemTier - 1].Add(craft.NeedItems);

            slot.AddItem(item);

            // 티어는 1부터 시작
            _resultItemSlotsList[item.itemTier - 1].AddSlotUI(slot);
        }

        UpdateNeedItemList(-1);
    }

    private void MoveInventory()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelectSlot(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelectSlot(Vector2.left);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelectSlot(Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelectSlot(Vector2.down);
        }
    }

    private void ChangeSelectSlot(Vector2 direction)
    {
        if (!CanMove(direction)) return;

        if (_isInResultItems)
        {
            _selectedResultItemSlots.MoveSelectSlot(direction);
            UpdateNeedItemList(_selectedResultItemSlots.SelectedSlotIndex);
        }
        else
        {
            _categorySlots.MoveSelectSlot(direction);
            _selectedResultItemSlots.gameObject.SetActive(false);
            _selectedResultItemSlots = _resultItemSlotsList[_categorySlots.SelectedSlotIndex];
            _selectedResultItemSlots.gameObject.SetActive(true);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        if (_isInResultItems)
        {

            if (direction.y == 1 && _selectedResultItemSlots.SelectedSlotIndex < _selectedResultItemSlots.LineCount)
            {
                GoCategory();
                return false;
            }
        }
        else
        {
            if (direction.y == -1)
            {
                if (_resultItemSlotsList[_categorySlots.SelectedSlotIndex].SlotUIs.Count <= 0)
                {
                    return false;
                }
                GoItemSlots();
                return false;
            }
        }

        return true;
    }

    private void GoItemSlots()
    {
        _categorySlots.Deactivate();

        _isInResultItems = true;
        _selectedResultItemSlots.gameObject.SetActive(false);
        _selectedResultItemSlots = _resultItemSlotsList[_categorySlots.SelectedSlotIndex];
        _selectedResultItemSlots.gameObject.SetActive(true);
        _selectedResultItemSlots.Activate();

        UpdateNeedItemList(_selectedResultItemSlots.SelectedSlotIndex);
    }

    private void GoCategory()
    {
        _selectedResultItemSlots.Deactivate();

        _isInResultItems = false;
        _categorySlots.Activate();

        _nameText.text = "";
        _descriptionText.text = "";
        UpdateNeedItemList(-1);
    }

    private void UpdateNeedItemList(int index)
    {
        if(index == -1)
        {
            if(_needItemSlots != null)
            {
                Destroy(_needItemSlots.gameObject);
            }
            _nameText.text = "";
            _descriptionText.text = "";
            return;
        }

        if(_needItemSlots != null)
        {
            Destroy(_needItemSlots.gameObject);
        }

        _needItemSlots = Instantiate(_itemSlotUIsPrefab, _needItemPanel.transform)
            .GetComponent<ItemSlotUIs>();
        _needItemSlots.SetPanelSize(new Vector2(5, 1));
        _needItemSlots.Deactivate();

        _canCraft = true;
        for (int i = 0; i < _needItemList[_categorySlots.SelectedSlotIndex][index].Count; i++)
        {
            Slot slot = new Slot(int.MaxValue);
            for (int j = 0; j < _needItemList[_categorySlots.SelectedSlotIndex][index][i].count; j++)
            {
                slot.AddItem(Manager.Data.ItemData.Values[_needItemList[_categorySlots.SelectedSlotIndex][index][i].ItemId]);
            }

            _needItemSlots.AddSlotUI(slot);

            if(!Manager.Game.ItemBox
                .IsItemExist(_needItemList[_categorySlots.SelectedSlotIndex][index][i].ItemId, _needItemList[_categorySlots.SelectedSlotIndex][index][i].count))
            {
                _needItemSlots.SlotUIs[i].SetColor(Color.red);
                _canCraft = false;
            }
        }

        Item item = _selectedResultItemSlots.SlotUIs[_selectedResultItemSlots.SelectedSlotIndex].Slot.CurItem;

        _nameText.text = item.itemName;
        _descriptionText.text = item.description;
    }
}
