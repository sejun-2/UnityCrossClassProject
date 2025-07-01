using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    public Transform topPoint;
    public Transform bottomPoint;

    private void Start()
    {
        Vector3 topPos = topPoint.position;
        topPos.z = 0;
        topPoint.position = topPos;
        Vector3 bottomPos = bottomPoint.position;
        bottomPos.z = 0;
        bottomPoint.position = bottomPos;
    }

    public void Interact()
    {

    }
    public void Interact(Transform player, bool goUp, float climbSpeed, PlayerInteraction interaction)
    {
        float distToTop = Vector3.Distance(player.position, topPoint.position);
        float distToBottom = Vector3.Distance(player.position, bottomPoint.position);

        if (goUp)
        {
            if (distToTop < distToBottom)
            {
                Debug.Log("이미 사다리 위에 있음. 위로 이동 불가.");
                return;
            }
        }
        else
        {
            if (distToBottom < distToTop)
            {
                Debug.Log("이미 사다리 아래에 있음. 아래로 이동 불가.");
                return;
            }
        }
        Vector3 from = goUp ? bottomPoint.position : topPoint.position;
        Vector3 to = goUp ? topPoint.position : bottomPoint.position;

        interaction.StartClimb(from, to);
    }
}
