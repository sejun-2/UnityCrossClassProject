using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint; // 공격 기준 위치 (손 위치 등)
    private SimpleSword currentWeapon; // 장착된 무기

    // 무기 장착 함수
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
