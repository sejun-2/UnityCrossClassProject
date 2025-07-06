using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private AudioClip _windSound;

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
            Manager.Sound.SfxPlayLoop("wind", _windSound, transform);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zoomOutCam.Priority = 0;
            Manager.Sound.SfxStopLoop("key", 0);
        }
    }
}
