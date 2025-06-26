using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    public Image ItemImage => _itemImage;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _outLine;

    [SerializeField] private Item _testItem;
    private Slot _slot = new();
    public Slot Slot => _slot;

    public void SetSlot(Slot slot)
    {
        _slot = slot;
        slot.OnItemChanged += UpdateSlotData;
        UpdateSlotData();
    }

    public void UpdateSlotData(Item item = null)
    {
        if(_slot.CurItem == null)
        {
            if(_itemImage != null)
                _itemImage.sprite = null;
        }
        else
        {
            if (_itemImage != null)
                _itemImage.sprite = _slot.CurItem.icon;
        }

        if(_slot.ItemCount > 1)
        {
            _countText.text = _slot.ItemCount.ToString();
        }
        else
        {
            _countText.text = "";
        }
    }

    public void UseItem()
    {
        if (_slot.IsEmpty) return;

        _slot.UseItem();
    }

    public Item GetItemData()
    {
        return _slot.CurItem;
    }

    public void Selected()
    {
        _outLine.gameObject.SetActive(true);
    }

    public void Unselected()
    {
        _outLine.gameObject.SetActive(false);
    }

    public void SetColor(Color color)
    {
        _countText.color = color;
    }
}
