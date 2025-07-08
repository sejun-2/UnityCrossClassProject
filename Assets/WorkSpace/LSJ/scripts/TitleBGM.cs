using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGM : MonoBehaviour
{
    public AudioSource audioSource; // Inspector���� �����ϰų� GetComponent�� �Ҵ�
    public AudioClip soundClip;     // Inspector���� ����� ���� ����

    void Start()
    {
        // AudioSource�� AudioClip �Ҵ�
        audioSource.clip = soundClip;
        // �Ҹ� ���
        audioSource.Play();
    }
}
