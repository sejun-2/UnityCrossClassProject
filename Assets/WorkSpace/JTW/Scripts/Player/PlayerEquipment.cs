using Newtonsoft.Json;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private AudioClip _equipSound;

    public void EquipmentWeapon(Item item)
    {
        if (Manager.Player.Stats.Weapon.Value != null)
        {
            UnEquipmentWeapon();
        }

        Manager.Player.Stats.Weapon.Value = item;
        Manager.Sound.SfxPlay(_equipSound, transform);
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
        Manager.Sound.SfxPlay(_equipSound, transform);
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
