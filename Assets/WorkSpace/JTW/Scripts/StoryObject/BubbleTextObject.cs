using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTextObject : MonoBehaviour, IInteractable
{
    [SerializeField] private List<string> dialogueId;

    public void Interact()
    {
        Manager.UI.Inven.ShowBubbleText(dialogueId);
        Manager.Player.Stats.isFarming = false;
    }
}
