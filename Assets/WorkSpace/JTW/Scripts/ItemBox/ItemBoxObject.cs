using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Manager.UI.Inven.ShowItemBox();
        Manager.Player.Stats.isFarming = true;
    }
}
