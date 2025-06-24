using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemDataManager
{
    public class ItemBase : ScriptableObject
    {
        //������ Ÿ�� ����
        public enum ItemType
        {
            Food, Drink, Material, Medicine, Potion, Weapon, Armor, Tool, Key, Furniture
        }
        public enum EquipmentType
        {
            Weapon, Armor, Tool
        }

        public string itemName;
        public ItemType itemType;
        public Sprite sprite;
        public string Description;
    }

    
    
    //public class PlaceableItem : ItemBase
    //{
    //    public GameObject prefabToPlace;
    //    public int placementCost;
    //}
    // ���� �������� ��� ȿ���� ������ �Լ�.
    //public abstract void Use();
}