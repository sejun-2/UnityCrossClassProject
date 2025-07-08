using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private float offsetX = 5f; // ī�޶� �¿� ����

    private CinemachineFramingTransposer transposer;
    private Transform player;

    void Start()
    {
        player = virtualCam.Follow;
        transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        // �÷��̾ �ٶ󺸴� ���� (localScale.x�� �Ǵ�)
        float dir = Mathf.Sign(player.localScale.x);

        Vector3 newOffset = transposer.m_TrackedObjectOffset;
        newOffset.x = dir * Mathf.Abs(offsetX);
        transposer.m_TrackedObjectOffset = newOffset;
    }
}
