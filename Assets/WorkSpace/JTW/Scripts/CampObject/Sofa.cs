using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip _interactionSound;

    public void Interact()
    {
        if (!Manager.Game.IsUsedObject["4003"])
        {
            Manager.Player.Stats.ChangeHp(20);
            Manager.Game.IsUsedObject["4003"] = true;
            Manager.Sound.SfxPlay(_interactionSound, Manager.Player.Transform);
        }
        else
        {
            Manager.UI.Inven.ShowBubbleText("20022", true);
        }

        Manager.Player.Stats.isFarming = false;
    }
}
