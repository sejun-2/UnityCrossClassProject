using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ �������� �� ���� �� �ִ� ��� ĳ���
public class HideSpot : MonoBehaviour
{
    // ĳ��� ������ ���� ��ġ
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
