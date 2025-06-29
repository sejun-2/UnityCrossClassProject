using UnityEditor.VisionOS;
using UnityEngine;


public partial class PlayerStats
{
    public void ChangeHunger(float amount)
    {
        float changeHunger = Hunger.Value + amount;

        if(changeHunger < 0)
        {
            Hunger.Value = 0;
        }else if(changeHunger > 100)
        {
            Hunger.Value = 100;
        }
        else
        {
            Hunger.Value = (int)changeHunger;
        }
    }

    public void ChangeThirst(float amount)
    {
        float changeThirst = Thirst.Value + amount;

        if (changeThirst < 0)
        {
            Thirst.Value = 0;
        }
        else if (changeThirst > 100)
        {
            Thirst.Value = 100;
        }
        else
        {
            Thirst.Value = (int)changeThirst;
        }
    }
}

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
    public int durabilityValue;
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
}
