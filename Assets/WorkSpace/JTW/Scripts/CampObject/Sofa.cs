using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (!Manager.Game.IsUsedObject["4003"])
        {
            Manager.Player.Stats.ChangeHp(20);
            Manager.Game.IsUsedObject["4003"] = true;
        }
        else
        {
            Manager.UI.Inven.ShowBubbleText("20022", true);
        }

        Manager.Player.Stats.isFarming = false;
    }
}
