using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TitleIntroController : MonoBehaviour
{
    public VideoPlayer videoPlayer;    // Inspector에서 VideoPlayer 연결
    public VideoClip[] clips;          // 영상 2개를 순서대로 할당
    public GameObject titleUI;         // 타이틀 UI 패널

    private int currentClipIndex = 0;

    void Start()
    {
        if (titleUI != null)
            titleUI.SetActive(false);

        videoPlayer.loopPointReached += OnEndReached;

        // 첫 번째 영상 재생
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
            // 다음 영상이 있으면 다음 영상 재생
            PlayClip(currentClipIndex + 1);
        }
        else
        {
            // 모든 영상 끝나면 UI 활성화
            if (titleUI != null)
                titleUI.SetActive(true);
            videoPlayer.gameObject.SetActive(false);
        }
    }
}
