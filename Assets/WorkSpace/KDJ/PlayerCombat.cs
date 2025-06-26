using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ����� �����ϴ� ��� ���
public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint; // ���� ���� ��ġ (�� ��ġ)
    public float attackRange = 2f; // ���� �Ÿ�
    public KeyCode attackKey = KeyCode.X; // ���� Ű
    public LayerMask enemyLayer; // �� Layer (��: Zombie)

    private IWeapon currentWeapon; // ���� ������ ����

    void Update()
    {
        // ���� Ű�� ���Ȱ� ���Ⱑ �����Ǿ� ���� ��
        if (Input.GetKeyDown(attackKey) && currentWeapon != null)
        {
            if (IsEnemyInFront()) // �տ� ���� ������
            {
                currentWeapon.Attack(attackPoint); // ���� ����
            }
            else
            {
                Debug.Log("���� ����: �տ� ���� ����");
            }
        }
    }

    // �ܺο��� ���� ����
    public void EquipWeapon(IWeapon weapon)
    {
        currentWeapon = weapon;
    }

    // Ray�� �տ� ���� �ִ��� Ȯ��
    bool IsEnemyInFront()
    {
        RaycastHit hit;
        Vector3 direction = attackPoint.forward;
        Debug.DrawRay(attackPoint.position, direction * attackRange, Color.red, 1f);

        if (Physics.Raycast(attackPoint.position, direction, out hit, attackRange, enemyLayer))
        {
            Debug.Log("�� Ž��: " + hit.collider.name);
            return true;
        }

        return false;
    }
}

