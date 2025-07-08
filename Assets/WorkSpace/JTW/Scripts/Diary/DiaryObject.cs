using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Manager.UI.Inven.ShowDiaryUI();
        Manager.Player.Stats.isFarming = true;
    }
}
