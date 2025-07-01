using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    private bool isHiding = false;             // 현재 숨은 상태 여부
    private HideSpot currentSpot = null;       // 현재 숨을 수 있는 장소
    private Renderer[] renderers;              // 플레이어 렌더러들
    private CharacterController controller;    // 플레이어 이동 막기용 (또는 직접 구현한 이동 스크립트)

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        controller = GetComponent<CharacterController>(); // 또는 Rigidbody, 직접 만든 PlayerMovement 등
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!isHiding)
                EnterHide();
            else
                ExitHide();
        }
    }

    // 캐비닛 안으로 숨기
    void EnterHide()
    {
        isHiding = true;

        // 플레이어 위치를 캐비닛 안으로 이동
        transform.position = currentSpot.hidePosition.position;

        // 렌더러 비활성화 (보이지 않게)
        SetRenderers(false);

        // 이동 제한
        if (controller != null)
            controller.enabled = false;

        Debug.Log("플레이어가 숨었습니다");
    }

    // 밖으로 나오기
    void ExitHide()
    {
        isHiding = false;

        SetRenderers(true);

        if (controller != null)
            controller.enabled = true;

        Debug.Log("플레이어가 나왔습니다");
    }

    // 렌더러 전체 켜거나 끄기
    void SetRenderers(bool value)
    {
        foreach (var rend in renderers)
        {
            rend.enabled = value;
        }
    }

    // 캐비닛과 충돌하면 기억해두기
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot))
        {
            currentSpot = spot;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot) && currentSpot == spot)
        {
            if (!isHiding) currentSpot = null;
        }
    }

    public bool IsHiding()
    {
        return isHiding;
    }
}
