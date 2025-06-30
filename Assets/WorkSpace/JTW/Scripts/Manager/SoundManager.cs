using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class SoundManager : Singleton<SoundManager>
{
    public ObjectPool<SfxController> SfxPool;

    public float MasterVolume = 1f;
    private float _bgmVolume = 1f;
    public float BgmVolume 
    { 
        get => _bgmVolume;
        set
        {
            _bgmVolume = value;
            _bgmSource.volume = MasterVolume * _bgmVolume * _bgmLocalVolume;
        }
    }
    private float _bgmLocalVolume;
    public float SfxVolume = 1f;

    private AudioSource _bgmSource;

    private void Awake()
    {
        _bgmSource = gameObject.GetOrAddComponent<AudioSource>();
        _bgmSource.loop = true;

        SfxPool = new ObjectPool<SfxController>(CreateSfx, GetSfx, ReleaseSfx, DestroySfx);
    }

    public void BgmPlay(AudioClip clip, float volume = 1f, float fadeDuration = 0)
    {
        _bgmLocalVolume = volume;
        _bgmSource.DOKill();
        _bgmSource.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            _bgmSource.Stop();
            _bgmSource.clip = clip;
            _bgmSource.Play();

            _bgmSource.DOFade(MasterVolume * BgmVolume * volume, fadeDuration);
        });
    }

    public void SfxPlay(AudioClip clip, Transform parent, float volume)
    {
        SfxController sfx = SfxPool.Get();
        sfx.transform.parent = parent;
        sfx.transform.localPosition = Vector3.zero;
        sfx.SfxPlay(clip, Mathf.Clamp01(MasterVolume * SfxVolume * volume));
    }

    private SfxController CreateSfx()
    {
        GameObject obj = new GameObject("SfxController");
        obj.transform.parent = transform;
        obj.AddComponent<AudioSource>();

        return obj.AddComponent<SfxController>();
    }

    private void GetSfx(SfxController sfx)
    {
        sfx.gameObject.SetActive(true);
    }

    private void ReleaseSfx(SfxController sfx)
    {
        sfx.transform.parent = transform;
        sfx.gameObject.SetActive(false);
    }

    private void DestroySfx(SfxController sfx)
    {
        Destroy(sfx.gameObject);
    }
}
