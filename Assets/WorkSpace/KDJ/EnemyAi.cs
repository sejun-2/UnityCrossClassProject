using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 AI가 플레이어를 탐지하는 기본 스크립트
public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;// 플레이어를 탐지할 수 있는 반경 (단위: 미터)
    public LayerMask playerLayer;// 플레이어를 포함하는 레이어 마스크 (Overlapping 체크용)

    void Update()
    {
        if (PlayerHide.IsHidden) // 플레이어가 숨은 상태일 경우, 탐지 로직을 건너뜀
        {
            return;// 플레이어가 캐비닛 등에서 은신 중이면 적이 무시함
        }

        // 플레이어가 숨지 않은 상태라면, 탐지 시도

        // 현재 위치를 기준으로 탐지 반경 내에서 'playerLayer'에 해당하는 오브젝트를 찾음
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        foreach (var hit in hits)// 탐지된 오브젝트들을 반복하며 검사
        {
            if (hit.CompareTag("Player"))// Player 태그를 가진 오브젝트가 있다면
            {
                Debug.Log("적이 플레이어를 발견함!");

                // 여기에 추적, 경고 상태 진입 등의 동작 추가 가능
                //상태 변경, 목표 지정, 사운드 재생 등
            }
        }
    }
}
