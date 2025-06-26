using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    public Transform topPoint;
    public Transform bottomPoint;
    /*
    public Transform GetTop() => topPoint;
    public Transform GetBottom() => bottomPoint;
    public float ExitRange => .6f;
    */
    public void Interact()
    {

    }
    public void RequestClimb(Transform player, bool goUp, float climbSpeed, PlayerInteraction interaction)
    {
        Vector3 from = goUp ? bottomPoint.position : topPoint.position;
        Vector3 to = goUp ? topPoint.position : bottomPoint.position;

        interaction.StartClimb(from, to);
    }
}
