using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private IInteractable interactable;

    private void Awake()
    {
        interactable = GetComponentInParent<IInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.Player.Stats.CurrentNearby = interactable;
            Debug.Log($"상호작용물체 {interactable}의 트리거 영역 진입");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Manager.Player.Stats.CurrentNearby == interactable)
        {
            Manager.Player.Stats.CurrentNearby = null;
            Debug.Log($"상호작용물체 {interactable}의 트리거 영역 퇴장");
        }
    }
}
