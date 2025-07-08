using UnityEngine;

public class ZombieSurprise : MonoBehaviour
{

    [SerializeField] Animator _animator;
    [SerializeField] AudioClip audioClip;
    [SerializeField] GameObject _zombieLyingPrefab;//누워있는 좀비 프리팹
    [SerializeField] GameObject _zombieStandingPrefab;//일어나서 깜놀시킬 좀비 프리팹

    private bool _hasSurprised = false;
    private Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();
        if (_col == null)
            Debug.LogError("Collider가 없습니다!");

        _animator = GetComponent<Animator>();
        _zombieStandingPrefab.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_hasSurprised) return;
        if (!other.CompareTag("Player")) return;
        _zombieLyingPrefab.SetActive(false);
        _zombieStandingPrefab.SetActive(true);
        _hasSurprised = true;                // 재생 플래그 설정
        _animator.Play("Surprise");          // 한 번만 깜짝 애니메이션
        Manager.Sound.SfxPlay(audioClip, transform, 1);

    }
}
