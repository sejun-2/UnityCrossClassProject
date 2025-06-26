using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적의 체력 및 데미지 처리 담당 클래스
public class EnemyStats : MonoBehaviour
{
    public float health = 50f; // 적의 체력

    // 플레이어 공격에 의해 데미지를 받는 함수
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("적이 데미지를 입음! 현재 체력: " + health);

        if (health <= 0f)
        {
            Die(); // 체력이 0 이하이면 사망 처리
        }
    }

    void Die()
    {
        Debug.Log("적이 사망했습니다.");
        Destroy(gameObject); // 적 게임 오브젝트 삭제
    }
}