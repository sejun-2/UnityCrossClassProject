using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 은신 상태를 제어하는 스크립트 (은신 지속 시간 제한 및 쿨타임 포함)
// 이 스크립트는 Renderer 컴포넌트가 있는 오브젝트에만 적용 가능
[RequireComponent(typeof(Renderer))]
public class PlayerStealth : MonoBehaviour
{
    //내부 상태 변수들
    private bool isHidden = false;        // 현재 은신 중인지 여부
    private bool canStealth = true;       // 은신 사용 가능한지 여부 
    private float stealthTimer = 0f;      // 은신 시간 누적용 타이머
    //은신 설정값
    public float maxStealthTime = 5f;     // 은신 가능한 최대 시간 초단위로
    public float stealthCooldown = 3f;    // 은신 해제 후 다시 은신 가능해지기까지 대기 시간
    //시각적 효과 설정
    private Renderer playerRenderer;      // 플레이어의 Renderer 컴포넌트 
    private Color originalColor;          // 원래 색상 저장 은신해제시에는 복원
    public Color hiddenColor = new Color(1, 1, 1, 0.3f); // 은신 시 색상 투명도도 포함

    // 게임 시작 시 초기화
    void Start()
    {
        playerRenderer = GetComponent<Renderer>(); // Renderer 컴포넌트 가져오기
        originalColor = playerRenderer.material.color;// 현재 색상 저장
    }

    // 매 프레임마다 호출
    void Update()
    {
        // H 키를 눌렀을 때 은신 시도
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isHidden && canStealth)// 은신 가능하고 현재 은신 중이 아니라면
            {
                ToggleStealth();// 은신 시작
            }
            else if (isHidden)// 은신 중일 경우 다시 누르면 해제
            {
                ToggleStealth();// 은신 해제
            }
        }

        if (isHidden)// 은신 중이라면 시간 누적
        {
            stealthTimer += Time.deltaTime;

            // 설정된 최대 은신 시간 초과 시 자동 해제
            if (stealthTimer >= maxStealthTime)
            {
                Debug.Log("은신 시간이 끝났습니다.");
                ToggleStealth(); // 은신 해제
                StartCoroutine(StealthCooldownRoutine()); // 쿨타임 시작
            }
        }
    }
    void ToggleStealth()// 은신 상태 토글 (켜기/끄기)
    {
        isHidden = !isHidden;// 은신 상태 반전

        if (isHidden)
        {
            EnterStealth();// 은신 시작 로직
        }
        else
        {
            ExitStealth();// 은신 해제 로직
        }

        Debug.Log("은신 상태: " + isHidden);
    }
    void EnterStealth()// 은신 시작 시 실행되는 함수
    {
        SetPlayerAlpha(hiddenColor.a);  // 투명도 조정
        stealthTimer = 0f;// 타이머 리셋
    }
    void ExitStealth()// 은신 해제 시 실행되는 함수
    {
        SetPlayerAlpha(originalColor.a); // 원래 투명도로 복구
    }

    void SetPlayerAlpha(float alpha)// Renderer를 통해 알파(투명도) 값 설정
    {
        Color c = playerRenderer.material.color;
        c.a = alpha;
        playerRenderer.material.color = c;
    }

    public bool IsHidden()// 외부에서 현재 은신 상태를 확인할 수 있도록 제공

    {
        return isHidden;
    }

    // 은신이 해제된 후 일정 시간 동안 다시 은신할 수 없도록 쿨타임을 적용하는 코루틴
    private System.Collections.IEnumerator StealthCooldownRoutine()
    {
        canStealth = false;// 은신 비활성화
        yield return new WaitForSeconds(stealthCooldown); // 쿨타임 대기
        canStealth = true; // 은신 다시 가능
        Debug.Log("은신 가능 상태로 복구됨");
    }
}

