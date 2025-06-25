using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    public Transform climbStartPoint;  // 사다리 타기 시작 위치
    public Transform climbEndPoint;    // 사다리 끝
    public Test player;
    public void Interact()
    {
        player.Climb();
    }
}
