using UnityEngine;


public enum ItemType
{
    Material = 0,
    Consumable = 1,
    Tool = 2,
    Weapon = 3,
    Armor = 4
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/General Item")]
public class Item : ScriptableObject
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


    // 실제 아이템의 사용 효과를 구현할 함수.
    public virtual void Use()
    {

    }
}
