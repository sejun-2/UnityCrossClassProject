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
        Vector3 from = goUp ? bottomPoint.position : topPoint.position;
        Vector3 to = goUp ? topPoint.position : bottomPoint.position;

        interaction.StartClimb(from, to);
    }
}
