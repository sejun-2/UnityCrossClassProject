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
            Debug.LogWarning("������ �Ǵ� �÷��̾ ����ֽ��ϴ�.");
            return;
        }

        if (item.hpRestore > 0)
            player.AddHp(item.hpRestore);

        if (item.hungerRestore > 0)
            player.AddHunger(item.hungerRestore);

        if (item.thirstRestore > 0)
            player.AddThirst(item.thirstRestore);

        Debug.Log($"������ '{item.itemName}' ����");
    }
}
