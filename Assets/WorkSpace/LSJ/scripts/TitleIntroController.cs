using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TitleIntroController : MonoBehaviour
{
    public VideoPlayer videoPlayer;    // Inspector���� VideoPlayer ����
    public VideoClip[] clips;          // ���� 2���� ������� �Ҵ�
    public GameObject titleUI;         // Ÿ��Ʋ UI �г�

    private int currentClipIndex = 0;

    void Start()
    {
        if (titleUI != null)
            titleUI.SetActive(false);

        videoPlayer.loopPointReached += OnEndReached;

        // ù ��° ���� ���
        PlayClip(0);
    }

    void PlayClip(int index)
    {
        if (clips != null && index < clips.Length && clips[index] != null)
        {
            videoPlayer.clip = clips[index];
            videoPlayer.Play();
            currentClipIndex = index;
        }
    }

    void OnEndReached(VideoPlayer vp)
    {
        if (currentClipIndex + 1 < clips.Length)
        {
            // ���� ������ ������ ���� ���� ���
            PlayClip(currentClipIndex + 1);
        }
        else
        {
            // ��� ���� ������ UI Ȱ��ȭ
            if (titleUI != null)
                titleUI.SetActive(true);
            videoPlayer.gameObject.SetActive(false);
        }
    }
}
