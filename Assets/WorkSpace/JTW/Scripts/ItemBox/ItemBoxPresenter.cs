using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemBoxPresenter : BaseUI
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
    private List<List<Item.ItemType>> _acceptTypeLists = new List<List<Item.ItemType>>();
    private ItemSlotUIs _selectedItemSlots;

    private ItemSlotUIs _categotySlots;

    private bool _isInItemSlots;

    private void Start()
    {
        // 각 카테고리에 들어갈 아이템 타입 선정
        _acceptTypeLists.Add(new List<Item.ItemType>() { Item.ItemType.Food, Item.ItemType.Drink, Item.ItemType.Medicine });
        _acceptTypeLists.Add(new List<Item.ItemType>() { Item.ItemType.Weapon, Item.ItemType.Armor });
        _acceptTypeLists.Add(new List<Item.ItemType>() { Item.ItemType.Material });
        _acceptTypeLists.Add(new List<Item.ItemType>() { Item.ItemType.Key });

        _itemSlotsPanel = GetUI("ItemSlotsPanel");
        _categoryPanel = GetUI("CategoryPanel");
        _itemNameText = GetUI<TextMeshProUGUI>("ItemNameText");
        _itemDescriptionText = GetUI<TextMeshProUGUI>("ItemDescriptionText");
    }

    private void Update()
    {
        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseItem();
        }
    }

    public void SetItemBoxData(ItemBoxData itemBox)
    {
        _itemBox = itemBox;
        InitItemBox();
    }

    private void InitItemBox()
    {
        _categotySlots = Instantiate(_itemSlotsPrefab, _categoryPanel.transform).GetComponent<ItemSlotUIs>();
        _categotySlots.GetComponent<RectTransform>().sizeDelta = new Vector2(520, 160);
        for(int i = 0; i < _categorySprites.Count; i++)
        {
            _categotySlots.AddSlotUI(maxItemCount:0);
            _categotySlots.SlotUIs[i].ItemImage.sprite = _categorySprites[i];
        }
        _categotySlots.SelectSlotUI(0);

        for(int i = 0; i < _categorySprites.Count; i++)
        {
            _itemSlotsList.Add(Instantiate(_itemSlotsPrefab, _itemSlotsPanel.transform).GetComponent<ItemSlotUIs>());
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

    public void AddItem(Item item)
    {
        foreach (Slot slot in _itemBox.SlotList)
        {
            if (slot.AddItem(item)) return;
        }

        Slot newSlot = new Slot(int.MaxValue);
        newSlot.AddItem(item);
        _itemBox.SlotList.Add(newSlot);

        foreach (ItemSlotUIs slots in _itemSlotsList)
        {
            if (slots.AddSlotUI(newSlot)) return;
        }
    }

    private void UseItem()
    {
        if (!_isInItemSlots) return;

        SlotUI selectedSlotUI = _selectedItemSlots.SlotUIs[_selectedItemSlots.SelectedSlotIndex];

        selectedSlotUI.UseItem();
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
            _itemNameText.text = item.Name;
            _itemDescriptionText.text = item.Description;
        }
            Destroy(_selectedItemSlots.gameObject);
        _itemSlotsList[_categotySlots.SelectedSlotIndex] = itemSlotUIs;
        _selectedItemSlots = itemSlotUIs;

        
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

        if (_isInItemSlots)
        {
            _selectedItemSlots.MoveSelectSlot(direction);
            Item item = _selectedItemSlots.SlotUIs[_selectedItemSlots.SelectedSlotIndex].Slot.CurItem;
            _itemNameText.text = item.Name;
            _itemDescriptionText.text = item.Description;
        }
        else
        {
            _categotySlots.MoveSelectSlot(direction);
            _selectedItemSlots.gameObject.SetActive(false);
            _selectedItemSlots = _itemSlotsList[_categotySlots.SelectedSlotIndex];
            _selectedItemSlots.gameObject.SetActive(true);

        }
    }

    private bool CanMove(Vector2 direction)
    {
        if (_isInItemSlots)
        {
            if(direction.y == 1 && _selectedItemSlots.SelectedSlotIndex < _selectedItemSlots.LineCount)
            {
                GoCategory();
                return false;
            }
        }
        else
        {
            if(direction.y == -1)
            {
                if (_itemSlotsList[_categotySlots.SelectedSlotIndex].SlotUIs.Count <= 0)
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
        _categotySlots.Deactivate();

        _isInItemSlots = true;
        _selectedItemSlots.gameObject.SetActive(false);
        _selectedItemSlots = _itemSlotsList[_categotySlots.SelectedSlotIndex];
        _selectedItemSlots.gameObject.SetActive(true);
        _selectedItemSlots.Activate();

        Item item = _selectedItemSlots.SlotUIs[_selectedItemSlots.SelectedSlotIndex].Slot.CurItem;
        _itemNameText.text = item.Name;
        _itemDescriptionText.text = item.Description;
    }

    private void GoCategory()
    {
        _selectedItemSlots.Deactivate();

        _isInItemSlots = false;
        _categotySlots.Activate();

        _itemNameText.text = "";
        _itemDescriptionText.text = "";
    }
}
