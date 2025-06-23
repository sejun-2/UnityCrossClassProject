using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemDataManager
{
    public class Item : ScriptableObject
    {
        //아이템 타입 종류
        public enum ItemType
        {
            Food, Drink, Weapon, Armor, Key, Material, Medicine, Furniture, Tool, Potion
        }

        public string itemName;
        public ItemType itemType;
        public Sprite sprite;
        public string Description;



        //체력회복
        public int hpRestore;
        //배고픔회복
        public int hungerRestore;
        //목마름회복
        public int thirstRestore;
        //추가공격력
        public int addAttackPower;
        //추가방어력
        public int addDefensePower;
        //내구도
        public int durability;
        //신선도
        public int freshness;
        //상태이상회복
        public bool cureStatusEffect;
        //갯수
        public int itemCount;

        // 실제 아이템의 사용 효과를 구현할 함수.
        //public abstract void Use();
    }
}