using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrafingObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Manager.UI.Inven.ShowCraftingUI();
        Manager.Player.Stats.isFarming = true;
    }
}
