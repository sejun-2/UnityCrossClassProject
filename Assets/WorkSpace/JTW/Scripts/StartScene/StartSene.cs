using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public class StartSene : MonoBehaviour
{
    [SerializeField] private VideoClip _video1;
    [SerializeField] private VideoClip _video2;

    [SerializeField] private AudioClip _audioClip;

    [SerializeField] private TextMeshProUGUI _startText;

    [SerializeField] private RawImage _renderImage;

    private VideoPlayer _vp;

    private bool _isReady = false;

    private void Awake()
    {
        _vp = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        _vp.loopPointReached += OnVideoEnded;
        _vp.clip = _video1;
        _vp.isLooping = false;
        _vp.Play();
    }

    private void Update()
    {
        if (_isReady && Input.anyKeyDown)
        {
            Manager.Game.ChangeScene("TitleScene");
        }
    }

    private void OnVideoEnded(VideoPlayer vp)
    {
        if(_vp.clip == _video1)
        {
            StartCoroutine(WaitCoroutine2(1f));
        }
        else
        {
            StartCoroutine(WaitCoroutine());
        }
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(1f);

        _startText.DOKill();
        _startText.DOFade(1f, 1f);

        _isReady = true;
    }
    private IEnumerator WaitCoroutine2(float time)
    {
        _renderImage.DOKill();
        _renderImage.DOFade(0f, 0f);
        yield return new WaitForSeconds(time);

        _vp.clip = _video2;
        _vp.Play();
        _renderImage.DOKill();
        _renderImage.DOFade(1f, 1f);
        Manager.Sound.BgmPlay(_audioClip);
    }
}
