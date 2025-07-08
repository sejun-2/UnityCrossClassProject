using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 바리케이드를 설치할 수 있도록 해주는 건설 시스템
public class BuildSystem : MonoBehaviour
{
    public GameObject barricadePrefab;// 설치할 바리케이드 프리팹 
    public LayerMask groundLayer;// 바닥으로 인식될 레이어 (Ray가 맞아야 설치됨)
    public KeyCode buildKey = KeyCode.B;// 설치키 기본: B 키
    void Update()
    {
        if (Input.GetKeyDown(buildKey))// 지정된 설치 키가 눌렸을 때 실행
        {
            // 카메라에서 마우스 위치 방향으로 광선(Ray)을 쏨
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Ray가 groundLayer에 속한 오브젝트와 부딪혔는지 검사 (최대 10미터 거리)
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, groundLayer))
            {
                // 부딪힌 위치에 바리케이드를 생성 (회전 없이 생성)
                Instantiate(barricadePrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
