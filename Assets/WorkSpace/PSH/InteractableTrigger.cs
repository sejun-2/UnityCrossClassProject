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
            Debug.Log($"��ȣ�ۿ빰ü {interactable}�� Ʈ���� ���� ����");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Manager.Player.Stats.CurrentNearby == interactable)
        {
            Manager.Player.Stats.CurrentNearby = null;
            Debug.Log($"��ȣ�ۿ빰ü {interactable}�� Ʈ���� ���� ����");
        }
    }
}
