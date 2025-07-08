using UnityEngine;

public class SilhouetteDisable : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
    }

    private void Start()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, ~0, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            OutLineVisible react = col.GetComponent<OutLineVisible>();
            if (react != null)
            {
                react.SetSilhouetteVisible();
            }
        }
    }

    private void OnDisable()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, ~0, QueryTriggerInteraction.Collide);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Interactable") || col.CompareTag("Ladder"))
            {
                //컴포넌트를 가지고 있다면 반응시킴
                OutLineVisible react = col.GetComponent<OutLineVisible>();
                if (react != null)
                {
                    react.SetSilhouetteInvisible();
                }
            }
        }
    }
}
