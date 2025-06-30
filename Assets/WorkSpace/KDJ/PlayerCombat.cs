using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;        // ���� ���� ��ġ (�� ��ġ ��)
    private SimpleSword currentWeapon;   // ������ ����
    public KeyCode attackKey = KeyCode.X;// ���� Ű
    public LayerMask enemyLayer;         // �� Layer (��: Zombie)
    public float checkRange = 2f;        // Ray�� ���� ������ �Ÿ�

    // ���� ���� �Լ�
    public void EquipWeapon(SimpleSword weapon)
    {
        currentWeapon = weapon;
    }

    void Update()
    {
        // ������ �ܼ� ���� Ű (Z)
        if (Input.GetKeyDown(KeyCode.Z) && currentWeapon != null)
        {
            currentWeapon.Attack(attackPoint);
        }

        // X Ű �Է� ��: �տ� ���� ���� ��츸 ���� ����
        if (Input.GetKeyDown(attackKey) && currentWeapon != null)
        {
            if (IsEnemyInFront()) // �տ� ���� �ִ��� Ȯ��
            {
                currentWeapon.Attack(attackPoint); // ���� ����
            }
            else
            {
                Debug.Log("���� ����: �տ� ���� ����");
            }
        }
    }

    // �÷��̾� �տ� ���� �ִ��� Ȯ���ϴ� �Լ� (Ray ���)
    private bool IsEnemyInFront()
    {
        RaycastHit hit;
        Vector3 direction = attackPoint.right; // ���� ���� = ���� �ٶ󺸴� ����

        // ����׿� �� �׸��� (Scene �信�� Ȯ�� ����)
        Debug.DrawRay(attackPoint.position, direction * checkRange, Color.red, 1f);

        // Ray�� ���� enemyLayer�� �ش��ϴ� ������Ʈ�� �¾Ҵ��� �˻�
        if (Physics.Raycast(attackPoint.position, direction, out hit, checkRange, enemyLayer))
        {
            Debug.Log("�� ������: " + hit.collider.name); // �ܼ� ���
            return true;
        }

        return false; // �� ����
    }
}
