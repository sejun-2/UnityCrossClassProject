using UnityEngine;
using System;

public enum ItemType
{
    Material = 0,
    Consumable = 1,
    Tool = 2,
    Weapon = 3,
    Armor = 4,
    Key = 5
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/General Item")]
public class Item : ScriptableObject, IUsableID
{
    public int index;
    public string itemName;
    public string description;
    public Sprite icon;
    public int itemTier;
    public ItemType itemType;

    public int attackValue;
    public int defValue;
    [SerializeField]
    private int _durubilityValue;
    public int durabilityValue 
    { 
        get => _durubilityValue; 
        set 
        { 
            _durubilityValue = value;
            OnDurabilityChanged?.Invoke(value); 
            if(_durubilityValue <= 0)
            {
                DestroyItem();
            }
        } 
    
    }
    public int maxDrabilityValue;
    public event Action<int> OnDurabilityChanged;
    public float attackSpeed;

    public int hpRecover;
    public int hungerRecover;
    public int thirstRecover;
    public int mentalRecover;

    public bool canStack;
    public int maxStackCount;

    public float attackRange;

    public string animationName;


    // 실제 아이템의 사용 효과를 구현할 함수.
    public virtual bool Use()
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                Manager.Player.Transform.GetComponent<PlayerEquipment>().EquipmentWeapon(this);
                return true;
            case ItemType.Armor:
                Manager.Player.Transform.GetComponent<PlayerEquipment>().EquipmentArmor(this);
                return true;
            case ItemType.Consumable:
                Manager.Player.Stats.ChangeHp(hpRecover);
                Manager.Player.Stats.ChangeHunger(hungerRecover);
                Manager.Player.Stats.ChangeThirst(thirstRecover);
                Manager.Player.Stats.ChangeMentality(mentalRecover);
                return true;
            case ItemType.Tool:
                return true;
            default:
                return false;
        }
    }

    public string GetID()
    {
        return index.ToString();
    }

    public void ClearEvent()
    {
        OnDurabilityChanged = null;
    }

    private void DestroyItem()
    {
        if(itemType == ItemType.Weapon)
        {
            Manager.Player.Stats.Weapon.Value = null;
        }
        else if(itemType == ItemType.Armor)
        {
            Manager.Player.Stats.Armor.Value = null;
        }
    }
}
