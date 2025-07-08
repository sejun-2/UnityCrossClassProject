using UnityEngine;

// 자동 나옴	일정 시간 숨고 나면 자동으로 나오는 시스템
// 그리고 적 AI 무시	IsHiding() 값을 적 AI가 인식하지 않도록 사용 가능 그리고
// 숨는 제한 시간 & 피로도 소모 그리고벽 너머 탐지 제한	Physics.Raycast로 장애물 탐지
// 플레이어가 은신처캐비닛에 들어가 숨고, 자동으로 나오는 기능을 구현한 스크립트
public class PlayerHide : MonoBehaviour
{
    private bool isHiding = false;// 현재 은신 중인지 여부
    private HideSpot currentSpot = null;// 플레이어가 접근한 은신처 참조
    private Renderer[] renderers;// 플레이어의 렌더러들 숨을때는 비활성화
    private CharacterController controller;// 이동 제어용 컴포넌트

    public float maxHideTime = 10f;// 최대 은신 유지 시간 
    public float fatiguePerSecond = 5f;// 은신 중 초당 피로도 감소량

    private float hideTimer = 0f;// 현재 은신 시간 누적
    private PlayerStats stats;// 피로도 등 플레이어 상태 관리 스크립트 참조

    public static bool IsHidden { get; private set; } // 다른 클래스에서 은신 여부 확인용

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();// 플레이어의 모든 렌더러 수집
        controller = GetComponent<CharacterController>();// 이동 제어 컴포넌트 참조
        stats = GetComponent<PlayerStats>();// 피로도 관리용 스크립트 참조
    }

    void Update()
    {
        // E 키를 누르고 은신처 근처에 있으면 숨거나 나옴
        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!isHiding)
                EnterHide(); // 숨기
            else
                ExitHide();  // 나오기
        }

        if (isHiding)// 은신 중일 경우: 타이머 및 피로도 감소 처리
        {
            hideTimer += Time.deltaTime; // 경과 시간 누적

            if (stats != null)// 피로도 감소
                stats.ReduceFatigue(fatiguePerSecond * Time.deltaTime);

            if (hideTimer >= maxHideTime)// 일정 시간이 지나면 자동으로 나옴
            {
                ExitHide();
                Debug.Log("은신 시간 초과로 자동 나옴");
            }
        }
    }

    void EnterHide()// 은신 시작 처리
    {
        isHiding = true;// 은신 상태 ON
        IsHidden = true;// 외부에서도 은신 상태로 인식 가능
        hideTimer = 0f;// 은신 타이머 초기화

        // 은신 위치로 이동 캐비냇안
        transform.position = currentSpot.hidePosition.position;

        SetRenderers(false);// 렌더러 끄기-보이지 않게
        if (controller != null) controller.enabled = false; // 움직임 제한

        Debug.Log("숨음");
    }

    void ExitHide()// 은신 해제 처리
    {
        isHiding = false;// 은신 상태 OFF
        IsHidden = false;

        SetRenderers(true);// 다시 보이게 하기
        if (controller != null) controller.enabled = true;  // 움직임 허용

        Debug.Log("나옴");
    }

    // 플레이어의 렌더러들을 켜거나 끄는 함수
    void SetRenderers(bool visible)
    {
        foreach (var rend in renderers)
        {
            rend.enabled = visible;
        }
    }

    // 은신처에 들어왔을 때 해당 은신처 정보 저장
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot))
        {
            currentSpot = spot;
        }
    }

    // 은신처를 벗어났을 때 정보 해제 숨고있는 경우엔 제외
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot) && currentSpot == spot && !isHiding)
        {
            currentSpot = null;
        }
    }

    // 외부에서 플레이어의 은신 상태를 확인할 수 있는 함수
    public static bool IsHiding()
    {
        return IsHidden;
    }
}

