using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 캐릭터의 체력 관리 스크립트
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;// 최대 체력 (인스펙터에서 설정 가능)
    private int currentHealth;
    void Start() => currentHealth = maxHealth;// 게임 시작 시 최대 체력으로 초기화
    public void TakeDamage(int amount)// 데미지를 받았을 때 호출되는 함수
    {
        currentHealth -= amount;// 체력 감소

        if (currentHealth <= 0)// 체력이 0 이하로 떨어지면 사망 처리
        {
            Die();
        }
    }
    void Die()// 적 사망 처리 함수
    {
        Destroy(gameObject);// 현재 오브젝트를 씬에서 제거 (또는 사망 애니메이션 후 제거 가능)
    }
}
