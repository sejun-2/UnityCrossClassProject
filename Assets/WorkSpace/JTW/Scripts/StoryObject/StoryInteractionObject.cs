using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInteractionObject : MonoBehaviour, IInteractable
{
    [SerializeField] string _storyInteractionId;

    public void Interact()
    {
        Manager.UI.Inven.ShowStoryInteractionPopUp(_storyInteractionId);
        Manager.Player.Stats.isFarming = true;
    }
}
