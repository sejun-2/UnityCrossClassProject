using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint; // ������ ���۵Ǵ� ��ġ (�÷��̾� ���� �� ��ġ ��)
    public float attackRange = 2f; // ���� �Ÿ�
    public KeyCode attackKey = KeyCode.X; // ���� �Է� Ű
    public LayerMask enemyLayer; // �� ���̾� ����ũ (Inspector���� "Enemy" ���̾ ����)

    private IWeapon currentWeapon; // ���� ������ ����

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && currentWeapon != null)
        {
            if (IsEnemyInFront())// ����ĳ��Ʈ�� ���� üũ�ϰ� ���� ���� ����
            {
                currentWeapon.Attack(attackPoint);
            }
            else
            {
                Debug.Log("�տ� ���� ���� - ���� ����");
            }
        }
    }

    public void EquipWeapon(IWeapon weapon)// ���⸦ �����ϴ� �Լ�
    {
        currentWeapon = weapon;
    }

    bool IsEnemyInFront() // �÷��̾� �տ� ���� �Ÿ� �� ���� �ִ��� Ȯ���ϴ� �Լ�
    {
        RaycastHit hit;
        Vector3 direction = attackPoint.forward; // ���� ����

        // ����׿� ���� ǥ�� (Scene �信�� Ȯ�� ����)
        Debug.DrawRay(attackPoint.position, direction * attackRange, Color.red, 1f);

        // Raycast�� ���� �ִ��� �˻�
        if (Physics.Raycast(attackPoint.position, direction, out hit, attackRange, enemyLayer))
        {
            // ���� ����ٸ� true ��ȯ
            Debug.Log("���� ������ϴ�: " + hit.collider.name);
            return true;
        }

        return false;
    }
}

