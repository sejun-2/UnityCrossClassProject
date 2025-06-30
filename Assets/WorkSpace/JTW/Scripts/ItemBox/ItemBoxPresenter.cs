using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBoxPresenter : BaseUI, IInventory
{
    [SerializeField] private GameObject _itemSlotsPrefab;
    [SerializeField] private GameObject _slotPrefab;
    [field: SerializeField] private List<Sprite> _categorySprites;

    private ItemBoxData _itemBox;

    private GameObject _categoryPanel;
    private GameObject _itemSlotsPanel;
    private TextMeshProUGUI _itemNameText;
    private TextMeshProUGUI _itemDescriptionText;

    private List<ItemSlotUIs> _itemSlotsList = new List<ItemSlotUIs>();
    private List<List<ItemType>> _acceptTypeLists = new List<List<ItemType>>();
    private ItemSlotUIs _selectedItemSlots;

    private ItemSlotUIs _categorySlots;

    private bool _isInItemSlots;

    private IInventory _inventoryForTrade;
    private Vector2 _tradeInvenDirection;
    private bool IsTrade => _inventoryForTrade != null;

    private bool _isActivate = true;
    private bool _isSwitchActivate = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (IsTrade)
            {
                Manager.UI.Inven.ShowMapUI();
            }
            Destroy(this.gameObject);
        }

        if (IsTrade && Input.GetKeyDown(KeyCode.C))
        {
            Manager.Game.ChangeScene(Manager.Game.SelectedMapName);
        }

        if (!_isActivate) return;

        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseItem();
        }
    }

    private void LateUpdate()
    {
        // Update 한 사이클이 끝나고 변환해야 MoveInventory가 정상 작동
        if (_isSwitchActivate)
        {
            _isActivate = !_isActivate;
            _isSwitchActivate = false;
        }
    }

    public void SetItemBoxData(ItemBoxData itemBox, IInventory tradeInven = null, Vector2 tradeDirection = default)
    {
        _itemBox = itemBox;
        if(tradeInven != null)
        {
            _inventoryForTrade = tradeInven;
            _tradeInvenDirection = tradeDirection;
        }

        InitData();
        InitItemBox();
    }

    private void InitData()
    {
        // 각 카테고리에 들어갈 아이템 타입 선정
        _acceptTypeLists.Add(new List<ItemType>() { ItemType.Consumable});
        _acceptTypeLists.Add(new List<ItemType>() { ItemType.Weapon, ItemType.Armor });
        _acceptTypeLists.Add(new List<ItemType>() { ItemType.Material });
        _acceptTypeLists.Add(new List<ItemType>() { ItemType.Tool });

        _itemSlotsPanel = GetUI("ItemSlotsPanel");
        _categoryPanel = GetUI("CategoryPanel");
        _itemNameText = GetUI<TextMeshProUGUI>("ItemNameText");
        _itemDescriptionText = GetUI<TextMeshProUGUI>("ItemDescriptionText");
    }

    private void InitItemBox()
    {
        _categorySlots = Instantiate(_itemSlotsPrefab, _categoryPanel.transform).GetComponent<ItemSlotUIs>();
        _categorySlots.SetPanelSize(new Vector2(5, 1));
        for(int i = 0; i < _categorySprites.Count; i++)
        {
            _categorySlots.AddSlotUI(maxItemCount:0);
            _categorySlots.SlotUIs[i].ItemImage.sprite = _categorySprites[i];
        }
        _categorySlots.SelectSlotUI(0);

        for(int i = 0; i < _categorySprites.Count; i++)
        {
            _itemSlotsList.Add(Instantiate(_itemSlotsPrefab, _itemSlotsPanel.transform).GetComponent<ItemSlotUIs>());
            _itemSlotsList[i].SetPanelSize(new Vector2(5, 4));
            _itemSlotsList[i].AcceptTypeList = _acceptTypeLists[i];
            _itemSlotsList[i].gameObject.SetActive(false);
        }

        _itemSlotsList[0].gameObject.SetActive(true);
        _selectedItemSlots = _itemSlotsList[0];

        foreach(Slot slot in _itemBox.SlotList)
        {
            foreach(ItemSlotUIs slots in _itemSlotsList)
            {
                if (slots.AddSlotUI(slot)) break;
            }
        }

        _itemNameText.text = "";
        _itemDescriptionText.text = "";
    }

    public bool AddItem(Item item)
    {
        foreach (Slot slot in _itemBox.SlotList)
        {
            if (slot.AddItem(item)) return true;
        }

        Slot newSlot = new Slot(int.MaxValue);
        newSlot.AddItem(item);
        _itemBox.SlotList.Add(newSlot);

        foreach (ItemSlotUIs slots in _itemSlotsList)
        {
            if (slots.AddSlotUI(newSlot)) return true;
        }

        return false;
    }

    private void UseItem()
    {
        if (!_isInItemSlots) return;

        SlotUI selectedSlotUI = _selectedItemSlots.SlotUIs[_selectedItemSlots.SelectedSlotIndex];

        Item item = selectedSlotUI.Slot.CurItem;

        if (item == null) return;

        if (IsTrade)
        {
            if(item.itemType == ItemType.Weapon || item.itemType == ItemType.Armor)
            {
                selectedSlotUI.UseItem();
            }
            else
            {
                if (!_inventoryForTrade.AddItem(item)) return;
                selectedSlotUI.Slot.RemoveItem();
            }
        }
        else
        {
            if (item.itemType == ItemType.Weapon || item.itemType == ItemType.Armor) return;

            selectedSlotUI.UseItem();
        }

        if (selectedSlotUI.Slot.IsEmpty)
        {
            _itemBox.SlotList.Remove(selectedSlotUI.Slot);
            _selectedItemSlots.SlotUIs.Remove(selectedSlotUI);

            ResetItemSlots();
        }
    }

    private void ResetItemSlots()
    {
        ItemSlotUIs itemSlotUIs = Instantiate(_itemSlotsPrefab, _itemSlotsPanel.transform)
                .GetComponent<ItemSlotUIs>();
        itemSlotUIs.SetPanelSize(new Vector2(5, 4));
        itemSlotUIs.AcceptTypeList = _selectedItemSlots.AcceptTypeList;
        foreach (SlotUI slotUI in _selectedItemSlots.SlotUIs)
        {
            itemSlotUIs.AddSlotUI(slotUI.Slot, int.MaxValue);
        }
        if (!itemSlotUIs.SelectSlotUI(_selectedItemSlots.SelectedSlotIndex))
        {
            GoCategory();
        }
        else
        {
            Item item = itemSlotUIs.SlotUIs[itemSlotUIs.SelectedSlotIndex].Slot.CurItem;
            _itemNameText.text = item.itemName;
            _itemDescriptionText.text = item.description;
        }
            Destroy(_selectedItemSlots.gameObject);
        _itemSlotsList[_categorySlots.SelectedSlotIndex] = itemSlotUIs;
        _selectedItemSlots = itemSlotUIs;

        
    }

    private void MoveInventory()
    {
        if (!_isActivate) return;

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

        if (_isInItemSlots)
        {
            _selectedItemSlots.MoveSelectSlot(direction);
            Item item = _selectedItemSlots.SlotUIs[_selectedItemSlots.SelectedSlotIndex].Slot.CurItem;
            _itemNameText.text = item.itemName;
            _itemDescriptionText.text = item.description;
        }
        else
        {
            _categorySlots.MoveSelectSlot(direction);
            _selectedItemSlots.gameObject.SetActive(false);
            _selectedItemSlots = _itemSlotsList[_categorySlots.SelectedSlotIndex];
            _selectedItemSlots.gameObject.SetActive(true);

        }
    }

    private bool CanMove(Vector2 direction)
    {
        if (_isInItemSlots)
        {
            if (IsTrade && direction == _tradeInvenDirection && _selectedItemSlots.CanChangeTrade(direction))
            {
                Deactivate();
                _inventoryForTrade.Activate(_selectedItemSlots.SelectedSlotIndex);
                return false;
            }

            if(direction.y == 1 && _selectedItemSlots.SelectedSlotIndex < _selectedItemSlots.LineCount)
            {
                GoCategory();
                return false;
            }
        }
        else
        {
            if (IsTrade && direction == _tradeInvenDirection && _categorySlots.CanChangeTrade(direction))
            {
                Deactivate();
                _inventoryForTrade.Activate(_categorySlots.SelectedSlotIndex);
                return false;
            }

            if (direction.y == -1)
            {
                if (_itemSlotsList[_categorySlots.SelectedSlotIndex].SlotUIs.Count <= 0)
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

        _isInItemSlots = true;
        _selectedItemSlots.gameObject.SetActive(false);
        _selectedItemSlots = _itemSlotsList[_categorySlots.SelectedSlotIndex];
        _selectedItemSlots.gameObject.SetActive(true);
        _selectedItemSlots.Activate();

        Item item = _selectedItemSlots.SlotUIs[_selectedItemSlots.SelectedSlotIndex].Slot.CurItem;
        _itemNameText.text = item.itemName;
        _itemDescriptionText.text = item.description;
    }

    private void GoCategory()
    {
        _selectedItemSlots.Deactivate();

        _isInItemSlots = false;
        _categorySlots.Activate();

        _itemNameText.text = "";
        _itemDescriptionText.text = "";
    }

    public void Activate(int index)
    {
        if (_isInItemSlots)
        {
            index = SetSelectIndex(_selectedItemSlots, index);
            _selectedItemSlots.Activate();
        }
        else
        {
            index = SetSelectIndex(_categorySlots, index);
            _categorySlots.Activate();
        }

        _isSwitchActivate = true;
    }

    private void Deactivate()
    {
        if (_isInItemSlots)
        {
            _selectedItemSlots.Deactivate();
        }
        else
        {
            _categorySlots.Deactivate();
        }

        _isSwitchActivate = true;
    }

    private int SetSelectIndex(ItemSlotUIs slotUIs, int index)
    {
        if (_tradeInvenDirection == Vector2.up)
        {
            return index % slotUIs.LineCount;
        }
        else if (_tradeInvenDirection == Vector2.down)
        {
            return (slotUIs.SlotUIs.Count + index) - slotUIs.LineCount;
        }
        else if (_tradeInvenDirection == Vector2.left)
        {
            int selectIndex = index - (slotUIs.LineCount - 1);

            while (selectIndex > slotUIs.SlotUIs.Count - 1)
            {
                selectIndex -= slotUIs.LineCount;
            }

            return selectIndex;
        }
        else if (_tradeInvenDirection == Vector2.right)
        {
            int selectIndex = index + (slotUIs.LineCount - 1);

            while (selectIndex > slotUIs.SlotUIs.Count - 1)
            {
                selectIndex -= slotUIs.LineCount;
            }

            if (selectIndex < 0)
            {
                selectIndex = slotUIs.SlotUIs.Count - 1;
            }

            return selectIndex;
        }

        return 0;
    }
}
