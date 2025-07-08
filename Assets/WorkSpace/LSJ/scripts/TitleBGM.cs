using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGM : MonoBehaviour
{
    public AudioSource audioSource; // Inspector에서 연결하거나 GetComponent로 할당
    public AudioClip soundClip;     // Inspector에서 오디오 파일 연결

    void Start()
    {
        // AudioSource에 AudioClip 할당
        audioSource.clip = soundClip;
        // 소리 재생
        audioSource.Play();
    }
}
