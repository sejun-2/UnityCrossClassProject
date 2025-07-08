using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    private string ObjectId = "4002";
    public void Interact()
    {
        if (!Manager.Game.IsUsedObject[ObjectId])
        {
            Manager.Player.Stats.ChangeMentality(20);
            Manager.Game.IsUsedObject[ObjectId] = true;
        }
        else
        {
            Manager.UI.Inven.ShowBubbleText("20022", true);
        }

        Manager.Player.Stats.isFarming = false;
    }
}
