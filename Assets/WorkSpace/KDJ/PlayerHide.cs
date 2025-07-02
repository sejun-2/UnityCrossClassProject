using UnityEngine;

// 플레이어가 은신처 캐비닛 에 숨거나 나오는 기능을 담당하는 스크립트
public class PlayerHide : MonoBehaviour
{
    private bool isHiding = false;              // 현재 플레이어가 숨은 상태인지 여부
    private HideSpot currentSpot = null;        // 현재 들어갈 수 있는 은신처 정보
    private Renderer[] renderers;               // 플레이어 외형 렌더러들 (숨을 때 끄기 위해)
    private CharacterController controller;     // 플레이어 이동 제어용 컴포넌트

    // 다른 스크립트에서 전체 은신 상태를 정적으로 확인할 수 있도록 제공 예로 적 AI 등에서 사용
    public static bool IsHidden { get; private set; }

    void Start()
    {
        // 플레이어 본체 및 자식에 포함된 모든 렌더러들을 가져옴
        renderers = GetComponentsInChildren<Renderer>();
        // 이동 제어용 CharacterController 컴포넌트를 가져옴
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // E 키를 눌렀고, 현재 숨을 수 있는 오브젝트가 근처에 있을 때
        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!isHiding)
                EnterHide(); // 숨기
            else
                ExitHide(); // 나오기
        }
    }
    void EnterHide()// 숨는 동작 수행
    {
        isHiding = true;
        IsHidden = true;

        // 플레이어 위치를 캐비닛 내부 좌표로 이동시킴
        transform.position = currentSpot.hidePosition.position;
        // 외형 렌더러를 끄기 → 플레이어가 안 보이도록
        SetRenderers(false);

        if (controller != null)// 이동 비활성화
            controller.enabled = false;

        Debug.Log("플레이어가 숨었습니다");
    }

    void ExitHide()// 숨은 상태에서 밖으로 나오는 동작
    {
        isHiding = false;
        IsHidden = false;
        SetRenderers(true);// 외형 렌더러 다시 켜기

        if (controller != null)// 이동 다시 활성화
            controller.enabled = true;

        Debug.Log("플레이어가 나왔습니다");
    }

    // 플레이어 외형의 렌더러들을 전부 켜거나 끄는 함수
    void SetRenderers(bool value)
    {
        foreach (var rend in renderers)
        {
            rend.enabled = value;
        }
    }
    // 캐비닛 등 은신 가능한 오브젝트에 닿았을 때 해당 위치 저장
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot))
        {
            currentSpot = spot;
        }
    }
    // 캐비닛 등에서 벗어났을 때 해당 위치 초기화
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot) && currentSpot == spot)
        {
            if (!isHiding) currentSpot = null;
        }
    }
    // 현재 은신 상태를 반환 (외부 접근용)
    public bool IsHiding()
    {
        return isHiding;
    }
}
