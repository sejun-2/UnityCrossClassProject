using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTextTrigger : MonoBehaviour
{
    [SerializeField] private List<string> _dialogueId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.UI.Inven.ShowBubbleText(_dialogueId);
        }
    }
}
