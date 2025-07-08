using UnityEngine;

public class ZombieSurprise : MonoBehaviour
{

    [SerializeField] Animator _animator;
    [SerializeField] AudioClip audioClip;
    [SerializeField] GameObject _zombieLyingPrefab;//�����ִ� ���� ������
    [SerializeField] GameObject _zombieStandingPrefab;//�Ͼ�� �����ų ���� ������

    private bool _hasSurprised = false;
    private Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();
        if (_col == null)
            Debug.LogError("Collider�� �����ϴ�!");

        _animator = GetComponent<Animator>();
        _zombieStandingPrefab.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_hasSurprised) return;
        if (!other.CompareTag("Player")) return;
        _zombieLyingPrefab.SetActive(false);
        _zombieStandingPrefab.SetActive(true);
        _hasSurprised = true;                // ��� �÷��� ����
        _animator.Play("Surprise");          // �� ���� ��¦ �ִϸ��̼�
        Manager.Sound.SfxPlay(audioClip, transform, 1);

    }
}
