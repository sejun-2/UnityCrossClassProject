using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� ���� (��) ����
public class SimpleSword : MonoBehaviour, IWeapon
{
    public float damage = 20f;           // ������ ��ġ
    public float range = 2f;             // ���� ����
    public LayerMask targetLayer;        // ���� ������ ���̾� (Zombie)

    // ���� ����
    public void Attack(Transform attackPoint)
    {
        // ���� �������� Ray �߻�
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out RaycastHit hit, range, targetLayer))
        {
            // �浹�� ������Ʈ�� ��(Zombie)�̸� ������ ������
            EnemyStats enemy = hit.collider.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ������ ó��
                Debug.Log($"{hit.collider.name}���� ���� ����!"); // �ܼ� �޽���
            }
        }
        else
        {
            Debug.Log("���� ����: �տ� ���� �����ϴ�.");
        }
    }
}