using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 무기로 공격하는 기능 담당
public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint; // 공격 기준 위치 (손 위치)
    public float attackRange = 2f; // 공격 거리
    public KeyCode attackKey = KeyCode.X; // 공격 키
    public LayerMask enemyLayer; // 적 Layer (예: Zombie)

    private IWeapon currentWeapon; // 현재 장착한 무기

    void Update()
    {
        // 공격 키가 눌렸고 무기가 장착되어 있을 때
        if (Input.GetKeyDown(attackKey) && currentWeapon != null)
        {
            if (IsEnemyInFront()) // 앞에 적이 있으면
            {
                currentWeapon.Attack(attackPoint); // 공격 실행
            }
            else
            {
                Debug.Log("공격 실패: 앞에 적이 없음");
            }
        }
    }

    // 외부에서 무기 장착
    public void EquipWeapon(IWeapon weapon)
    {
        currentWeapon = weapon;
    }

    // Ray로 앞에 적이 있는지 확인
    bool IsEnemyInFront()
    {
        RaycastHit hit;
        Vector3 direction = attackPoint.forward;
        Debug.DrawRay(attackPoint.position, direction * attackRange, Color.red, 1f);

        if (Physics.Raycast(attackPoint.position, direction, out hit, attackRange, enemyLayer))
        {
            Debug.Log("적 탐지: " + hit.collider.name);
            return true;
        }

        return false;
    }
}

