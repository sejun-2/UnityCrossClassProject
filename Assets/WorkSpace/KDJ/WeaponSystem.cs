using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 근접 무기 + 방어구 기능 통합 시스템
// 이 스크립트는 무기의 공격, 내구도 감소, 방어구의 피해 처리까지 담당함
public class WeaponSystem : MonoBehaviour
{
    //공격 관련 설정
    [Header("공격 관련 설정")]
    public float attackRange = 20f;            // 무기의 공격 범위 (구 형태로 계산됨)
    public float attackCooldown = 5f;          // 공격 간의 쿨타임 (초 단위)
    public int durability = 1;                 // 무기의 현재 내구도 (사용 시 감소함)
    public LayerMask enemyLayer;               // 공격 대상이 될 적의 레이어 마스크

    //방어 관련 설정

    [Header("방어 관련 설정")]
    public int armorDurability = 1;            // 방어구의 현재 내구도 (피격 시 감소)
    private float lastAttackTime = -Mathf.Infinity;

    // 무기를 사용할 수 있는지 여부를 반환하는 함수
    // 조건: 쿨타임이 지났고, 내구도가 남아있어야 함
    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown && durability > 0;
    }

    // 공격 수행 함수
    // 공격 위치(attackPoint) 주변에 적이 있으면 데미지를 입힘
    public void Attack(Transform attackPoint)
    {
        if (!CanAttack()) return;// 공격 불가능한 상태면 종료
        lastAttackTime = Time.time;// 현재 시간 기록 (쿨타임 계산용)
        durability--;// 무기 내구도 1 감소

        // 공격 범위 내 적 감지 (구형 범위 안에 있는 적 Collider 배열 반환)
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPoint.position,        // 중심점: 공격 위치
            attackRange,                 // 반지름: 공격 거리
            enemyLayer                   // 레이어 마스크: 적만 감지
        );

        // 감지된 적들에게 데미지 전달
        foreach (Collider enemy in hitEnemies)
        {
            // EnemyHealth 스크립트가 있다면 데미지를 입힘
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(1);
        }

        // 디버그 로그 출력: 공격 성공 및 내구도 표시
        Debug.Log("무기 휘두름! 남은 내구도: " + durability);

        if (durability <= 0)// 내구도가 0 이하가 되면 무기 파괴
        {
            Debug.Log("무기 파괴됨!");
            gameObject.SetActive(false); // 오브젝트 비활성화 (또는 Destroy(gameObject))
        }
    }

    // 적에게 공격을 당했을 때 호출되는 함수
    // 방어구 내구도가 1씩 감소하며, 0이 되면 파괴됨
    public void TakeHit()
    {
        // 방어구 내구도가 남아있을 때만 처리
        if (armorDurability > 0)
        {
            armorDurability--; // 내구도 1 감소
            Debug.Log("피격! 방어구 내구도 감소. 남은 방어구: " + armorDurability);
        }

        // 내구도가 0이 되면 방어구 파괴 처리
        if (armorDurability <= 0)
        {
            Debug.Log("방어구가 파괴되었습니다!");
            // 필요 시 방어구 비활성화, 파괴 애니메이션 등 추가 가능
        }
    }

    // 공격 범위를 Scene 뷰에서 시각화하는 디버그용 함수
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위를 빨간색 원으로 그림
    }
}
