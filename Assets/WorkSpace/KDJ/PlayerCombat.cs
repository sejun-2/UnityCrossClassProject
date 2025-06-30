using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;        // 공격 기준 위치 (손 위치 등)
    private SimpleSword currentWeapon;   // 장착된 무기
    public KeyCode attackKey = KeyCode.X;// 공격 키
    public LayerMask enemyLayer;         // 적 Layer (예: Zombie)
    public float checkRange = 2f;        // Ray로 적을 감지할 거리

    // 무기 장착 함수
    public void EquipWeapon(SimpleSword weapon)
    {
        currentWeapon = weapon;
    }

    void Update()
    {
        // 디버깅용 단순 공격 키 (Z)
        if (Input.GetKeyDown(KeyCode.Z) && currentWeapon != null)
        {
            currentWeapon.Attack(attackPoint);
        }

        // X 키 입력 시: 앞에 적이 있을 경우만 공격 실행
        if (Input.GetKeyDown(attackKey) && currentWeapon != null)
        {
            if (IsEnemyInFront()) // 앞에 적이 있는지 확인
            {
                currentWeapon.Attack(attackPoint); // 공격 실행
            }
            else
            {
                Debug.Log("공격 실패: 앞에 적이 없음");
            }
        }
    }

    // 플레이어 앞에 적이 있는지 확인하는 함수 (Ray 사용)
    private bool IsEnemyInFront()
    {
        RaycastHit hit;
        Vector3 direction = attackPoint.right; // 공격 방향 = 손이 바라보는 방향

        // 디버그용 선 그리기 (Scene 뷰에서 확인 가능)
        Debug.DrawRay(attackPoint.position, direction * checkRange, Color.red, 1f);

        // Ray를 쏴서 enemyLayer에 해당하는 오브젝트가 맞았는지 검사
        if (Physics.Raycast(attackPoint.position, direction, out hit, checkRange, enemyLayer))
        {
            Debug.Log("적 감지됨: " + hit.collider.name); // 콘솔 출력
            return true;
        }

        return false; // 적 없음
    }
}
