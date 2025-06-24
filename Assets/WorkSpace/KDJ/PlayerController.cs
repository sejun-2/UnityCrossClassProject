using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;//걷기 속도
    public float climbSpeed = 3f; // 사다리 타기 속도
    // 플레이어 Rigidbody
    private Rigidbody rb;

    public Transform cam;// 카메라 참조 (이동 방향 및 회전에 사용)
    private bool isClimbing = false;// 사다리에 붙어 있는지 여부를 판단하는 플래그

    void Start()
    {
        rb = GetComponent<Rigidbody>();// Rigidbody 컴포넌트 가져오기
        Cursor.lockState = CursorLockMode.Locked;// 마우스 커서를 화면 중앙에 고정
    }

    void Update()
    {
        // 사다리에 붙어 있는 경우 위/아래 이동
        if (isClimbing)
        {
            Climb();
        }
        // 아닌 경우 좌우 이동
        else
        {
            MoveSideways();
        }
        Rotate();// 항상 카메라 방향을 따라 회전
    }
    void MoveSideways()
    {
        // A/D 키 또는  왼쪽오른쪽 입력값 (-1 ~ 1)
        float h = Input.GetAxis("Horizontal");

        // 카메라의 오른쪽 방향을 기준으로 이동 벡터 계산
        Vector3 move = cam.right * h;
        move.y = 0;// Y축(수직)은 제거해서 수평 이동만 하도록 설정
        transform.position += move.normalized * moveSpeed * Time.deltaTime; // 실제 이동 처리
    }

    // 사다리에서 위/아래 이동 처리 함수
     void Climb()
    {
        // W/S 키 또는 위아래 입력값 (-1 ~ 1)
        float v = Input.GetAxis("Vertical");
        Vector3 climb = Vector3.up * v;// 위 방향으로만 이동 벡터 설정
        transform.position += climb * climbSpeed * Time.deltaTime;// 실제 이동 처리
    }

    // 카메라 방향을 기준으로 플레이어 회전 처리
    void Rotate()
    {
        Vector3 lookDir = cam.forward;// 카메라의 정면 방향을 기준으로 회전
        lookDir.y = 0;// 위/아래 방향 제거 (Y축은 회전하지 않음)
        transform.forward = lookDir; // 플레이어의 앞 방향 설정
    }

    // 사다리 영역에 들어갔을 때 호출됨 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;         // 사다리 상태 활성화
            rb.useGravity = false;     // 중력 비활성화 
            rb.velocity = Vector3.zero; // 기존 속도 제거
        }
    }
    private void OnTriggerExit(Collider other)// 사다리 영역에서 나왔을 때 호출됨
    {
        // 태그가 "Ladder"인 오브젝트에서 벗어난 경우
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;       // 사다리 상태 해제
            rb.useGravity = true;     // 중력 다시 활성화
        }
    }
}
