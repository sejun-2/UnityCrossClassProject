using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private float offsetX = 5f; // 카메라 좌우 길이

    private CinemachineFramingTransposer transposer;
    private Transform player;

    void Start()
    {
        player = virtualCam.Follow;
        transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        // 플레이어가 바라보는 방향 (localScale.x로 판단)
        float dir = Mathf.Sign(player.localScale.x);

        Vector3 newOffset = transposer.m_TrackedObjectOffset;
        newOffset.x = dir * Mathf.Abs(offsetX);
        newOffset.y = 1.3f;
        transposer.m_TrackedObjectOffset = newOffset;
    }
}
