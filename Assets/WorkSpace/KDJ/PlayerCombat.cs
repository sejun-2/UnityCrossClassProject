using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint; // ���� ���� ��ġ (�� ��ġ ��)
    private SimpleSword currentWeapon; // ������ ����

    // ���� ���� �Լ�
    public void EquipWeapon(SimpleSword weapon)
    {
        currentWeapon = weapon;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && currentWeapon != null)
        {
            currentWeapon.Attack(attackPoint);
        }
    }
}
