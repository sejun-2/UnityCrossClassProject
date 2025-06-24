using ItemDataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableItem : ItemBase
{
    //ü��ȸ��
    public int hpRestore;
    //�����ȸ��
    public int hungerRestore;
    //�񸶸�ȸ��
    public int thirstRestore;
    //����
    public int itemCount;
    //�����̻�ȸ��
    public bool cureStatusEffect;
    //�ż���
    public int freshness;
}
