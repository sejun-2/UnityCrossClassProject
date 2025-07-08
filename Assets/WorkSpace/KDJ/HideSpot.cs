using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 접근했을 때 숨을 수 있는 장소 캐비닛
public class HideSpot : MonoBehaviour
{
    // 캐비닛 안으로 숨을 위치
    public Transform hidePosition;

    void OnDrawGizmos()
    {
        if (hidePosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hidePosition.position, 0.2f);
        }
    }
}
