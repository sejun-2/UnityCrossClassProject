using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �⺻���� ���� ���� ���� 
public class WeaponSystem : MonoBehaviour
{
    public float attackRange = 20f;      // ���� �Ÿ�
    public float attackCooldown = 5f;     // ��Ÿ�� 
    public int durability = 5;            // ������
    public LayerMask enemyLayer;          // ���� ��� ���̾�

    private float lastAttackTime = -Mathf.Infinity;
    public bool CanAttack() // ���� ��� �������� üũ (��Ÿ�� & ������)
    {
        return Time.time >= lastAttackTime + attackCooldown && durability > 0;
    }

    public void Attack(Transform attackPoint)// ���� ����
    {
        if (!CanAttack()) return;

        lastAttackTime = Time.time;// ��Ÿ�� ���
        durability--; // ������ ����

        // �ֺ� �� Ž��
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(1);// ������ ������ ����
        }
        Debug.Log("���� �ֵθ�! ���� ������: " + durability);// �ܼ� ���

        if (durability <= 0)// ������ 0�̸� �ı� �Ǵ� ��Ȱ��ȭ ó��
        {
            Debug.Log("���� �ı���!");
            gameObject.SetActive(false); // �Ǵ� Destroy(gameObject);
        }
    }
    private void OnDrawGizmosSelected()// ���� ���� �ð�ȭ (�����Ϳ�)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
