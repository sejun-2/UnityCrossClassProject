using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public enum ItemType
    {
        Food, Drink, Weapon, Armor, Key, Material, Medicine, Furniture
    }

    public string Name;
    public string Description;
    public Sprite Sprite;
    public ItemType Type;

    // 실제 아이템의 사용 효과를 구현할 함수.
    public abstract void Use();
}