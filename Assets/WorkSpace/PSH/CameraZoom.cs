using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera zoomOutCam;
    [SerializeField] private Transform _playerCamera;
    public List<ParallaxEffect> parals = new List<ParallaxEffect>();

    private void Start()
    {
        zoomOutCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zoomOutCam.Priority = 20;
            foreach(ParallaxEffect paral in parals)
            {
                paral.ChangeSubject(zoomOutCam.transform);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zoomOutCam.Priority = 0;
            foreach (ParallaxEffect paral in parals)
            {
                paral.ChangeSubject(_playerCamera);
            }
        }
    }
}
