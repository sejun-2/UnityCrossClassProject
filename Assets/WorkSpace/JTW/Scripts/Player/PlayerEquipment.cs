using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStats
{
    public Stat<Item> Weapon;
    public Stat<Item> Armor;
}

public class PlayerEquipment : MonoBehaviour
{
    public void EquipmentWeapon(Item item)
    {
        if (Manager.Player.Stats.Weapon.Value != null)
        {
            UnEquipmentWeapon();
        }

        Manager.Player.Stats.Weapon.Value = item;
    }

    public void UnEquipmentWeapon()
    {
        Manager.Game.Inven.AddItem(Manager.Player.Stats.Weapon.Value);
        Manager.Player.Stats.Weapon.Value = null;
    }

    public void EquipmentArmor(Item item)
    {
        if (Manager.Player.Stats.Armor.Value != null)
        {
            UnEquipmentArmor();
        }

        Manager.Player.Stats.Armor.Value = item;
    }

    public void UnEquipmentArmor()
    {
        Manager.Game.Inven.AddItem(Manager.Player.Stats.Armor.Value);
        Manager.Player.Stats.Armor.Value = null;
    }
}
