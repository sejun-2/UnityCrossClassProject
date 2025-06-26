using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ����� ����ϴ� Ŭ���� (IWeapon �������̽� ����)
public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public float attackRange = 1.5f;// ���� ���� ������ (���� ���� ����)
    public int damage = 20;// ������ ������ ��ġ
    public LayerMask enemyLayer;// ���� ����� �� ���̾� (��: Enemy�� ���Ե� Layer)
    public void Attack(Transform attackPoint)// ���� ���� ���� �Լ� (IWeapon �������̽� ����)
    {
        // ���� ���� ���� ���� �ִ��� ����
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        // ������ ��� ������ �ݺ������� ������ ó��
        foreach (Collider hit in hits)
        {
            // EnemyHealth ������Ʈ�� ���� ���� ������ ����
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
