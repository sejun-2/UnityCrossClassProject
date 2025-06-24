using ItemDataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equipment")]
public class EquipmentItem : ItemBase
{
    public EquipmentType equipmentType;
    //추가공격력
    public int addAttackPower;
    //추가방어력
    public int addDefensePower;
    //내구도
    public int durability;
}
