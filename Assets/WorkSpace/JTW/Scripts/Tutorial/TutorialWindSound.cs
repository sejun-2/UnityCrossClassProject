using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindSound : MonoBehaviour
{
    [SerializeField] private AudioClip _windSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.Sound.SfxPlayLoop("wind", _windSound, Manager.Player.Transform);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.Sound.SfxStopLoop("wind", 0);
        }
    }
}
