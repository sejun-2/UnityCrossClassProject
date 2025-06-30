using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    public Image ItemImage => _itemImage;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _outLine;
    [SerializeField] private Slider _durabilitySlider;

    [SerializeField] private Item _testItem;
    private Slot _slot = new();
    public Slot Slot => _slot;

    public void SetSlot(Slot slot)
    {
        _slot = slot;
        slot.OnItemChanged += UpdateSlotData;
        slot.OnItemChanged += UpdateDurabilityData;
        UpdateSlotData();
        UpdateDurabilityData(slot.CurItem);

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

    public void UpdateDurabilityData(Item item)
    {
        if (_durabilitySlider == null) return;

        if (item != null && (item.itemType == ItemType.Weapon || item.itemType == ItemType.Armor))
        {
            _durabilitySlider.gameObject.SetActive(true);
            UpdateDurabilitySlider(item.durabilityValue);
            item.OnDurabilityChanged += UpdateDurabilitySlider;
        }
        else
        {
            _durabilitySlider.gameObject.SetActive(false);
        }
    }

    private void UpdateDurabilitySlider(int value)
    {
        _durabilitySlider.value = _slot.CurItem.durabilityValue / _slot.CurItem.maxDrabilityValue;
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
        if(_itemImage != null)
        {
            _itemImage.color = color;
        }
        _countText.color = color;
    }

    public Vector2 GetSlotSize()
    {
        RectTransform rt = transform.GetComponent<RectTransform>();

        return rt.sizeDelta;
    }

    public void SetText(string text)
    {
        _countText.text = text;
    }
}
