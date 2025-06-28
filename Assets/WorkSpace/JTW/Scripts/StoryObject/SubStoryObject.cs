using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStoryObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string _storyId;

    public void Interact()
    {
        Manager.Player.Stats.isFarming = true;
        Manager.Game.IsGetSubStory[_storyId] = true;
    }
}
