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

    public void UpdateSlotData(Item item = null)
    {
        _slot.RemoveItem();

        if (item == null) return;

        _slot.AddItem(item);

        if (_itemImage != null)
            _itemImage.sprite = _slot.CurItem.icon;
    }

    public void UpdateDurabilityData(Item item = null)
    {
        if (_durabilitySlider == null) return;

        if (item != null)
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
}
