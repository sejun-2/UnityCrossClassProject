using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemDataManager
{
    public class ItemBase : ScriptableObject
    {
        //아이템 타입 종류
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
    // 실제 아이템의 사용 효과를 구현할 함수.
    //public abstract void Use();
}