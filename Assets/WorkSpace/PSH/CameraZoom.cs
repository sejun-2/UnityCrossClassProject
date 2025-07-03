using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera zoomOutCam;

    private void Start()
    {
        zoomOutCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zoomOutCam.Priority = 20;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zoomOutCam.Priority = 0;
        }
    }
}
