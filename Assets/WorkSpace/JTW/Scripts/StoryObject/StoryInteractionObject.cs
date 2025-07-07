using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInteractionObject : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip _interactionSound;
    [SerializeField] string _storyInteractionId;

    public void Interact()
    {
        Manager.Sound.SfxPlay(_interactionSound, Manager.Player.Transform);
        Manager.UI.Inven.ShowStoryInteractionPopUp(_storyInteractionId);
        Manager.Player.Stats.isFarming = true;
    }
}
