using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTextObject : MonoBehaviour, IInteractable
{
    [SerializeField] private List<string> dialogueId;
    [SerializeField] private bool _isLoop = false;

    public void Interact()
    {
        Manager.UI.Inven.ShowBubbleText(dialogueId, _isLoop);
        Manager.Player.Stats.isFarming = false;
    }
}
