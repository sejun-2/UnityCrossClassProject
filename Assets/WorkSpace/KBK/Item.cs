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

    // ���� �������� ��� ȿ���� ������ �Լ�.
    public abstract void Use();
}