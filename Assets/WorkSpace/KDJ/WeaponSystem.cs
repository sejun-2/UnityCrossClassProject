using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본적인 근접 무기 구현 
public class WeaponSystem : MonoBehaviour
{
    public float attackRange = 20f;      // 공격 거리
    public float attackCooldown = 5f;     // 쿨타임 
    public int durability = 5;            // 내구도
    public LayerMask enemyLayer;          // 공격 대상 레이어

    private float lastAttackTime = -Mathf.Infinity;
    public bool CanAttack() // 무기 사용 가능한지 체크 (쿨타임 & 내구도)
    {
        return Time.time >= lastAttackTime + attackCooldown && durability > 0;
    }

    public void Attack(Transform attackPoint)// 공격 수행
    {
        if (!CanAttack()) return;

        lastAttackTime = Time.time;// 쿨타임 기록
        durability--; // 내구도 감소

        // 주변 적 탐색
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(1);// 적에게 데미지 전달
        }
        Debug.Log("무기 휘두름! 남은 내구도: " + durability);// 콘솔 출력

        if (durability <= 0)// 내구도 0이면 파괴 또는 비활성화 처리
        {
            Debug.Log("무기 파괴됨!");
            gameObject.SetActive(false); // 또는 Destroy(gameObject);
        }
    }
    private void OnDrawGizmosSelected()// 공격 범위 시각화 (에디터용)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
