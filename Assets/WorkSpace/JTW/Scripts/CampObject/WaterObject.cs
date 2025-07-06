using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (!Manager.Game.IsUsedObject["4004"])
        {
            Manager.Game.ItemBox.AddItem(Instantiate(Manager.Data.ItemData.Values["10028"]));
            Manager.Game.IsUsedObject["4004"] = true;
        }
        else
        {
            Manager.UI.Inven.ShowBubbleText("20022", true);
        }

        Manager.Player.Stats.isFarming = false;
    }
}
