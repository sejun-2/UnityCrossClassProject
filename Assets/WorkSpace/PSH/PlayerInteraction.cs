using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 2f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 1f;
            float dirX = Mathf.Sign(transform.localScale.x); // 바라보는 방향: -1(left), +1(right)
            Vector3 rayDir = new Vector3(dirX, 0f, 0f);
            float rayLength = interactDistance;

            Debug.DrawRay(rayOrigin, rayDir * rayLength, Color.green, 0.2f);

            Ray ray = new Ray(rayOrigin, rayDir);

            if (Physics.Raycast(ray, out RaycastHit hit, rayLength, interactableLayer, QueryTriggerInteraction.Collide))
            {
                IInteractable target = hit.collider.GetComponentInParent<IInteractable>();
                if (target != null)
                {
                    target.Interact();
                }
                Debug.Log("Hit: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Ray가 아무것도 못 맞춤");
            }
        }


        //이동 임시로
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * 5 * Time.deltaTime);
            Vector3 scale = transform.localScale;
            scale.x = -1f;
            transform.localScale = scale;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * 5 * Time.deltaTime);
            Vector3 scale = transform.localScale;
            scale.x = 1f;
            transform.localScale = scale;
        }
    }
}
