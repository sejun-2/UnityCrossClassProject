using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� AI�� �÷��̾ Ž���ϴ� �⺻ ��ũ��Ʈ
public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;// �÷��̾ Ž���� �� �ִ� �ݰ� (����: ����)
    public LayerMask playerLayer;// �÷��̾ �����ϴ� ���̾� ����ũ (Overlapping üũ��)

    void Update()
    {
        if (PlayerHide.IsHidden) // �÷��̾ ���� ������ ���, Ž�� ������ �ǳʶ�
        {
            return;// �÷��̾ ĳ��� ��� ���� ���̸� ���� ������
        }

        // �÷��̾ ���� ���� ���¶��, Ž�� �õ�

        // ���� ��ġ�� �������� Ž�� �ݰ� ������ 'playerLayer'�� �ش��ϴ� ������Ʈ�� ã��
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        foreach (var hit in hits)// Ž���� ������Ʈ���� �ݺ��ϸ� �˻�
        {
            if (hit.CompareTag("Player"))// Player �±׸� ���� ������Ʈ�� �ִٸ�
            {
                Debug.Log("���� �÷��̾ �߰���!");

                // ���⿡ ����, ��� ���� ���� ���� ���� �߰� ����
                //���� ����, ��ǥ ����, ���� ��� ��
            }
        }
    }
}
