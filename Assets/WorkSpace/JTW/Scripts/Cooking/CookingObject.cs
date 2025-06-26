using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Manager.UI.Inven.ShowCookingUI();
        Manager.Player.Stats.isFarming = true;
    }
}
