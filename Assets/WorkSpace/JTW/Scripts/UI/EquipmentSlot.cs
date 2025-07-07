using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    public Image ItemImage => _itemImage;
    [SerializeField] private Slider _durabilitySlider;

    [SerializeField] private ItemType _equipmentType;

    private Slot _slot = new();
    public Slot Slot => _slot;

    private void Awake()
    {
        SetSlot();
    }

    public void SetSlot()
    {
        _slot = new Slot();

        if(_equipmentType == ItemType.Weapon)
        {
            Manager.Player.Stats.Weapon.OnChanged += UpdateSlotData;
            Manager.Player.Stats.Weapon.OnChanged += UpdateDurabilityData;

            UpdateSlotData(Manager.Player.Stats.Weapon.Value);
            UpdateDurabilityData(Manager.Player.Stats.Weapon.Value);
        }
        else if(_equipmentType == ItemType.Armor)
        {
            Manager.Player.Stats.Armor.OnChanged += UpdateSlotData;
            Manager.Player.Stats.Armor.OnChanged += UpdateDurabilityData;

            UpdateSlotData(Manager.Player.Stats.Armor.Value);
            UpdateDurabilityData(Manager.Player.Stats.Armor.Value);
        }
    }

    public void UpdateSlotData(Item item)
    {
        if (_itemImage != null)
        {
            if (item != null)
            {
                _itemImage.gameObject.SetActive(true);
                _itemImage.sprite = item.icon;
            }
            else
            {
                _itemImage.gameObject.SetActive(false);
            }
        }

        _slot.RemoveItem();

        if (item == null) return;

        _slot.AddItem(item);

    }

    public void UpdateDurabilityData(Item item)
    {
        Debug.Log("ddd");

        if (_durabilitySlider == null) return;

        if (item != null)
        {
            Debug.Log("dddwwwww");
            _durabilitySlider.gameObject.SetActive(true);
            UpdateDurabilitySlider(item.durabilityValue);
            item.OnDurabilityChanged += UpdateDurabilitySlider;
        }
        else
        {
            Debug.Log("dddwwwwaaaa");
            _durabilitySlider.gameObject.SetActive(false);
        }
    }

    private void UpdateDurabilitySlider(int value)
    {
        Debug.Log("dddasdassdasdasdd");
        _durabilitySlider.value = (float)_slot.CurItem.durabilityValue / _slot.CurItem.maxDrabilityValue;
    }
}
