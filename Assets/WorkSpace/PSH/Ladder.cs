using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    public Transform climbStartPoint;  // ��ٸ� Ÿ�� ���� ��ġ
    public Transform climbEndPoint;    // ��ٸ� ��
    public Test player;
    public void Interact()
    {
        player.Climb();
    }
}
