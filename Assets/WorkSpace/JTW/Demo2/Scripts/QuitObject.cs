using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Manager.Game.DayComplete();
        Manager.Game.ChangeScene("BaseCamp");
    }
}
