using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsv2 : MonoBehaviour
{
    public float health = 100f;// 플레이어의 현재 체력 (0~100)
    public float hunger = 100f;// 배고픔 수치 (0이면 굶주림 상태, 100은 만복)
    public float fatigue = 0f;// 피로도 수치 (0이면 상쾌, 100이면 탈진)
    public float mental = 100f;// 정신력 수치 (정신 붕괴 시스템 구현 예정)
    public float hungerDecayRate = 5f;// 배고픔 감소 속도 (초당 5씩 감소)
    public float fatigueIncreaseRate = 5f;// 피로 증가 속도 (초당 5씩 증가)

    void Update()
    {
        hunger -= hungerDecayRate * Time.deltaTime;// 배고픔은 시간이 지날수록 감소
        fatigue += fatigueIncreaseRate * Time.deltaTime;// 피로도는 시간이 지날수록 증가
        // 배고픔과 피로도를 0~100 범위로 고정
        hunger = Mathf.Clamp(hunger, 0f, 100f);
        fatigue = Mathf.Clamp(fatigue, 0f, 100f);

        // 배고픔이 0이거나 피로가 100 이상이면 체력 감소 시작
        if (hunger <= 0 || fatigue >= 100f)
        {
            health -= Time.deltaTime * 5f;// 체력은 초당 5씩 감소
        }

        if (health <= 0f)// 체력이 0 이하로 떨어지면 사망 처리
        {
            Die();
        }
    }
    void Die()// 플레이어 사망 시 호출되는 함수
    {
        Debug.Log("플레이어 사망");  // 콘솔에 사망 메시지 출력
        // 실제 게임에선 여기에 게임 오버 UI, 리스폰 등 추가 가능
    }
}
