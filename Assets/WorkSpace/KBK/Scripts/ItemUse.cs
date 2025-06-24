using ItemDataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour
{
    public PlayerStatus player;
    public ConsumableItem hpItem;
    public ConsumableItem hungerItem;
    public ConsumableItem thirstItem;

    public void UseHpItem()
    {
        ApplyItem(hpItem);
    }

    public void UseHungerItem()
    {
        ApplyItem(hungerItem);
    }

    public void UseThirstItem()
    {
        ApplyItem(thirstItem);
    }

    void ApplyItem(ConsumableItem item)
    {
        if (item == null || player == null)
        {
            Debug.LogWarning("아이템 또는 플레이어가 비어있습니다.");
            return;
        }

        if (item.hpRestore > 0)
            player.AddHp(item.hpRestore);

        if (item.hungerRestore > 0)
            player.AddHunger(item.hungerRestore);

        if (item.thirstRestore > 0)
            player.AddThirst(item.thirstRestore);

        Debug.Log($"아이템 '{item.itemName}' 사용됨");
    }
}
