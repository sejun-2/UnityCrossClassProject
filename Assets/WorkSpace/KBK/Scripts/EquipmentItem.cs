using ItemDataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equipment")]
public class EquipmentItem : ItemBase
{
    public EquipmentType equipmentType;
    //�߰����ݷ�
    public int addAttackPower;
    //�߰�����
    public int addDefensePower;
    //������
    public int durability;
}
