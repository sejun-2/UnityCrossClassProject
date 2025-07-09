using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField] private Sprite _tutorialSprite;
    [SerializeField] private bool _isIgnoreTutorial = false;

    private List<GameObject> _tutorialUIs = new List<GameObject>();

    private IInteractable interactable;

    private void Awake()
    {
        interactable = GetComponentInParent<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isIgnoreTutorial)
            {
                _tutorialUIs.Add(Manager.UI.Inven.ShowTutorialUI(transform, _tutorialSprite));
            }
            Manager.Player.Stats.CurrentNearby = interactable;
            Debug.Log($"상호작용물체 {interactable}의 트리거 영역 진입");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(GameObject obj in _tutorialUIs)
            {
                Destroy(obj);
            }

            if(Manager.Player.Stats.CurrentNearby == interactable)
            {
                Manager.Player.Stats.CurrentNearby = null;
                Debug.Log($"상호작용물체 {interactable}의 트리거 영역 퇴장");
            }
        }
    }
}
