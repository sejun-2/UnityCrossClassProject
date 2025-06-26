using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 근접 무기 기능을 담당하는 클래스 (IWeapon 인터페이스 구현)
public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public float attackRange = 1.5f;// 공격 범위 반지름 (무기 공격 범위)
    public int damage = 20;// 무기의 데미지 수치
    public LayerMask enemyLayer;// 공격 대상이 될 레이어 (예: Enemy만 포함된 Layer)
    public void Attack(Transform attackPoint)// 무기 공격 실행 함수 (IWeapon 인터페이스 구현)
    {
        // 공격 범위 내에 적이 있는지 감지
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        // 감지된 모든 적에게 반복적으로 데미지 처리
        foreach (Collider hit in hits)
        {
            // EnemyHealth 컴포넌트를 가진 적만 데미지 적용
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
