using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFarming : MonoBehaviour, IInteractable
{
    private Inventory _farmingInven;

    private bool _isInit;

    private void Awake()
    {
        _farmingInven = new Inventory(new Vector2(5, 1));
    }

    public void Interact()
    {
        if (!_isInit)
        {
            Item bat = Manager.Data.ItemData.Values["10014"];
            bat.durabilityValue = 1;
            _farmingInven.AddItem(bat);
            _isInit = true;
        }

        Manager.UI.Inven.ShowTradeFarming(_farmingInven);
        Manager.Player.Stats.isFarming = true;
    }
}
