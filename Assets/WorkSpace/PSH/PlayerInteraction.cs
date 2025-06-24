using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 2f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
            {
                IInteractable target = hit.collider.GetComponent<IInteractable>();
                if (target != null)
                {
                    target.Interact();
                }
            }
        }
    }
}
