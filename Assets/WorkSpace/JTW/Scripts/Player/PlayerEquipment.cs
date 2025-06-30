using Newtonsoft.Json;
using UnityEngine;

public partial class PlayerStats
{
    [JsonIgnore]
    public Stat<Item> Weapon = new();
    [JsonIgnore]
    public Stat<Item> Armor = new();
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
        if (Manager.Game.IsInBaseCamp)
        {
            Manager.Game.ItemBox.AddItem(Manager.Player.Stats.Weapon.Value);
        }
        else
        {
            Manager.Game.Inven.AddItem(Manager.Player.Stats.Weapon.Value);
        }

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
        if (Manager.Game.IsInBaseCamp)
        {
            Manager.Game.ItemBox.AddItem(Manager.Player.Stats.Armor.Value);
        }
        else
        {
            Manager.Game.Inven.AddItem(Manager.Player.Stats.Armor.Value);
        }

        Manager.Player.Stats.Armor.Value = null;
    }
}
