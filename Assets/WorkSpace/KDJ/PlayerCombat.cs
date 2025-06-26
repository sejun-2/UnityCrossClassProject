using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint; // 공격이 시작되는 위치 (플레이어 앞쪽 손 위치 등)
    public float attackRange = 2f; // 공격 거리
    public KeyCode attackKey = KeyCode.X; // 공격 입력 키
    public LayerMask enemyLayer; // 적 레이어 마스크 (Inspector에서 "Enemy" 레이어를 지정)

    private IWeapon currentWeapon; // 현재 장착한 무기

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && currentWeapon != null)
        {
            if (IsEnemyInFront())// 레이캐스트로 적을 체크하고 있을 때만 공격
            {
                currentWeapon.Attack(attackPoint);
            }
            else
            {
                Debug.Log("앞에 적이 없음 - 공격 실패");
            }
        }
    }

    public void EquipWeapon(IWeapon weapon)// 무기를 장착하는 함수
    {
        currentWeapon = weapon;
    }

    bool IsEnemyInFront() // 플레이어 앞에 일정 거리 내 적이 있는지 확인하는 함수
    {
        RaycastHit hit;
        Vector3 direction = attackPoint.forward; // 공격 방향

        // 디버그용 레이 표시 (Scene 뷰에서 확인 가능)
        Debug.DrawRay(attackPoint.position, direction * attackRange, Color.red, 1f);

        // Raycast로 적이 있는지 검사
        if (Physics.Raycast(attackPoint.position, direction, out hit, attackRange, enemyLayer))
        {
            // 적을 맞췄다면 true 반환
            Debug.Log("적을 맞췄습니다: " + hit.collider.name);
            return true;
        }

        return false;
    }
}

