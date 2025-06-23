using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _outLine;

    [SerializeField] private Item _testItem;
    private Slot _slot = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _slot.GetItem(_testItem);
            UpdateSlotData();
        }
    }


    public void SetSlot(Slot slot)
    {
        _slot = slot;
        UpdateSlotData();
    }

    public void UpdateSlotData()
    {
        if(_slot.CurItem == null)
        {
            _itemImage.sprite = null;
        }
        else
        {
            _itemImage.sprite = _slot.CurItem.Sprite;
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
        UpdateSlotData();
    }
    public void Selected()
    {
        _outLine.gameObject.SetActive(true);
    }

    public void Unselected()
    {
        _outLine.gameObject.SetActive(false);
    }
}
