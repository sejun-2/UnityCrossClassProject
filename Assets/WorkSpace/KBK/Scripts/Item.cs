using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemDataManager
{
    public class Item : ScriptableObject
    {
        //������ Ÿ�� ����
        public enum ItemType
        {
            Food, Drink, Weapon, Armor, Key, Material, Medicine, Furniture, Tool, Potion
        }

        public string itemName;
        public ItemType itemType;
        public Sprite sprite;
        public string Description;



        //ü��ȸ��
        public int hpRestore;
        //�����ȸ��
        public int hungerRestore;
        //�񸶸�ȸ��
        public int thirstRestore;
        //�߰����ݷ�
        public int addAttackPower;
        //�߰�����
        public int addDefensePower;
        //������
        public int durability;
        //�ż���
        public int freshness;
        //�����̻�ȸ��
        public bool cureStatusEffect;
        //����
        public int itemCount;

        // ���� �������� ��� ȿ���� ������ �Լ�.
        //public abstract void Use();
    }
}