using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotUIs : MonoBehaviour
{
    [SerializeField] private GameObject _slotUIPrefab;

    private List<SlotUI> _slotUIs = new List<SlotUI>();
    public List<SlotUI> SlotUIs => _slotUIs;
    private Vector2 _slotUIPosition = new Vector2(30, -30);
    private float _interval = 120;

    private int _lineCount = 4;
    public int LineCount => _lineCount;

    private SlotUI _selectedSlot;
    private int _selectedSlotIndex;
    public int SelectedSlotIndex => _selectedSlotIndex;

    public List<Item.ItemType> AcceptTypeList = new List<Item.ItemType>();

    public bool AddSlotUI(Slot slot = null, int maxItemCount = 4)
    {
        if(AcceptTypeList.Count != 0 && !AcceptTypeList.Contains(slot.CurItem.Type)) return false;

        RectTransform rt = Instantiate(_slotUIPrefab, transform).GetComponent<RectTransform>();
        rt.anchoredPosition = _slotUIPosition;
        SlotUI slotUI = rt.GetComponent<SlotUI>();
        if(slot != null)
        {
            slotUI.SetSlot(slot);
        }
        else
        {
            slotUI.SetSlot(new Slot(maxItemCount));
        }
        _slotUIs.Add(slotUI);

        if(_slotUIs.Count % _lineCount == 0)
        {
            _slotUIPosition.y -= _interval;
            _slotUIPosition.x = 30;
        }
        else
        {
            _slotUIPosition.x += _interval;
        }

        return true;
    }

    public bool SelectSlotUI(int index)
    {
        if (SlotUIs.Count <= 0) return false;

        if (index < 0) index = 0;
        if (index >= SlotUIs.Count) index = SlotUIs.Count - 1;

        if(_selectedSlot != null)
        {
            _selectedSlot.Unselected();
        }
        _selectedSlotIndex = index;
        _selectedSlot = _slotUIs[index];
        _selectedSlot.Selected();

        return true;
    }

    public bool MoveSelectSlot(Vector2 direction)
    {
        int moveIndex = _selectedSlotIndex + (int)(direction.x - direction.y * _lineCount);

        if (moveIndex < 0 || moveIndex >= _slotUIs.Count) return false;

        SelectSlotUI(moveIndex);

        return true;
    }

    public void Activate()
    {
        SelectSlotUI(_selectedSlotIndex);
    }

    public void Deactivate()
    {
        if(_selectedSlot != null)
        {
            _selectedSlot.Unselected();
        }
    }

    public bool CanChangeTrade(Vector2 direction)
    {
        if (direction.x == 1)
        {
            if((_selectedSlotIndex + 1) % _lineCount == 0) return true;
        }
        else if(direction.x == -1)
        {
            if (_selectedSlotIndex % _lineCount == 0) return true;
        }
        else if(direction.y == 1)
        {
            if (_selectedSlotIndex < _lineCount) return true;
        }
        else if(direction.y == -1)
        {
            int minIndex = ((_slotUIs.Count - 1) / _lineCount) * _lineCount;

            if (_selectedSlotIndex >= minIndex && _selectedSlotIndex < _slotUIs.Count) return true;
        }

        return false;
    }
}
