using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 간단한 근접 무기 (검) 구현
public class SimpleSword : MonoBehaviour, IWeapon
{
    public float damage = 20f;           // 데미지 수치
    public float range = 2f;             // 공격 범위
    public LayerMask targetLayer;        // 공격 가능한 레이어 (Zombie)

    // 공격 실행
    public void Attack(Transform attackPoint)
    {
        // 공격 방향으로 Ray 발사
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out RaycastHit hit, range, targetLayer))
        {
            // 충돌한 오브젝트가 적(Zombie)이면 데미지 입히기
            EnemyStats enemy = hit.collider.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 데미지 처리
                Debug.Log($"{hit.collider.name}에게 공격 성공!"); // 콘솔 메시지
            }
        }
        else
        {
            Debug.Log("공격 실패: 앞에 적이 없습니다.");
        }
    }
}