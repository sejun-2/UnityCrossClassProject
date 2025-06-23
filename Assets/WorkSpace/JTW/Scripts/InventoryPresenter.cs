using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : BaseUI
{
    [SerializeField] private Item _testItemPrefab;

    [SerializeField] private GameObject _slotUIPrefab;
    [SerializeField] private Inventory _inven = new();

    private Vector2 _slotStartPos = new Vector2(30, -30);
    private float _interval = 120f;

    private SlotUI[,] _slotUIs;
    private SlotUI _selectedSlot;
    private Vector2 _selectedSlotPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetInventory(new Inventory(_testItemPrefab));
        }

        MoveInventory();
        GetUseInput();
    }

    public void SetInventory(Inventory inven)
    {
        _inven = inven;
        InitInventory();
    }

    public void InitInventory()
    {
        _slotUIs = new SlotUI[_inven.SlotList.GetLength(0), _inven.SlotList.GetLength(1)];
        for(int i = 0; i < _inven.SlotList.GetLength(0); i++)
        {
            for(int j = 0; j < _inven.SlotList.GetLength(1); j++)
            {
                RectTransform rt = Instantiate(_slotUIPrefab, transform).GetComponent<RectTransform>();
                rt.anchoredPosition = _slotStartPos;
                SlotUI slot = rt.GetComponent<SlotUI>();
                slot.SetSlot(_inven.SlotList[i, j]);
                _slotUIs[i, j] = slot;

                _slotStartPos.x += _interval;
            }
            _slotStartPos.y -= _interval;
            _slotStartPos.x = 30;
        }

        _selectedSlot = _slotUIs[0, 0];
        _selectedSlotPos = Vector2.zero;
        _selectedSlot.Selected();
    }

    private void GetUseInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _selectedSlot.UseItem();
        }
    }

    private void MoveInventory()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelectSlot(_selectedSlotPos + Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelectSlot(_selectedSlotPos + Vector2.left);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelectSlot(_selectedSlotPos - Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelectSlot(_selectedSlotPos - Vector2.down);
        }
    }

    private void ChangeSelectSlot(Vector2 movePos)
    {
        if (CanMove(movePos))
        {
            _selectedSlot.Unselected();
            _selectedSlotPos = movePos;
            _selectedSlot = _slotUIs[(int)movePos.y, (int)movePos.x];
            _selectedSlot.Selected();
        }
    }

    private bool CanMove(Vector2 pos)
    {
        if (pos.x < 0 || pos.y < 0 ||
            pos.x >= _slotUIs.GetLength(0) || pos.y >= _slotUIs.GetLength(1))
        {
            return false;
        }

        return true;
    }
}
