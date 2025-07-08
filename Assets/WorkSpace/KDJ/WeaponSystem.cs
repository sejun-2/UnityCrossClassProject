using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� + �� ��� ���� �ý���
// �� ��ũ��Ʈ�� ������ ����, ������ ����, ���� ���� ó������ �����
public class WeaponSystem : MonoBehaviour
{
    //���� ���� ����
    [Header("���� ���� ����")]
    public float attackRange = 20f;            // ������ ���� ���� (�� ���·� ����)
    public float attackCooldown = 5f;          // ���� ���� ��Ÿ�� (�� ����)
    public int durability = 1;                 // ������ ���� ������ (��� �� ������)
    public LayerMask enemyLayer;               // ���� ����� �� ���� ���̾� ����ũ

    //��� ���� ����

    [Header("��� ���� ����")]
    public int armorDurability = 1;            // ���� ���� ������ (�ǰ� �� ����)
    private float lastAttackTime = -Mathf.Infinity;

    // ���⸦ ����� �� �ִ��� ���θ� ��ȯ�ϴ� �Լ�
    // ����: ��Ÿ���� ������, �������� �����־�� ��
    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown && durability > 0;
    }

    // ���� ���� �Լ�
    // ���� ��ġ(attackPoint) �ֺ��� ���� ������ �������� ����
    public void Attack(Transform attackPoint)
    {
        if (!CanAttack()) return;// ���� �Ұ����� ���¸� ����
        lastAttackTime = Time.time;// ���� �ð� ��� (��Ÿ�� ����)
        durability--;// ���� ������ 1 ����

        // ���� ���� �� �� ���� (���� ���� �ȿ� �ִ� �� Collider �迭 ��ȯ)
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,        // �߽���: ���� ��ġ
            attackRange,                 // ������: ���� �Ÿ�
            enemyLayer                   // ���̾� ����ũ: ���� ����
        );

        // ������ ���鿡�� ������ ����
        foreach (Collider enemy in hitEnemies)
        {
            // EnemyHealth ��ũ��Ʈ�� �ִٸ� �������� ����
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(1);
        }

        // ����� �α� ���: ���� ���� �� ������ ǥ��
        Debug.Log("���� �ֵθ�! ���� ������: " + durability);

        if (durability <= 0)// �������� 0 ���ϰ� �Ǹ� ���� �ı�
        {
            Debug.Log("���� �ı���!");
            gameObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ (�Ǵ� Destroy(gameObject))
        }
    }

    // ������ ������ ������ �� ȣ��Ǵ� �Լ�
    // �� �������� 1�� �����ϸ�, 0�� �Ǹ� �ı���
    public void TakeHit()
    {
        // �� �������� �������� ���� ó��
        if (armorDurability > 0)
        {
            armorDurability--; // ������ 1 ����
            Debug.Log("�ǰ�! �� ������ ����. ���� ��: " + armorDurability);
        }

        // �������� 0�� �Ǹ� �� �ı� ó��
        if (armorDurability <= 0)
        {
            Debug.Log("���� �ı��Ǿ����ϴ�!");
            // �ʿ� �� �� ��Ȱ��ȭ, �ı� �ִϸ��̼� �� �߰� ����
        }
    }

    // ���� ������ Scene �信�� �ð�ȭ�ϴ� ����׿� �Լ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // ���� ������ ������ ������ �׸�
    }
}
