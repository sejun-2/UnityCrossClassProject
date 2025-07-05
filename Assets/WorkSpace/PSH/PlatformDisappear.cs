using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDisappear : MonoBehaviour
{
    [SerializeField] private float delay = .1f; // ������������ ������
    [SerializeField] private GameObject _particlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(_particlePrefab, transform.position + new Vector3(3f, 0, 1.5f), transform.rotation);
        gameObject.SetActive(false); // ��Ȱ��ȭ
        // Destroy(gameObject); // �����ϰ� �ʹٸ� ��� ���
    }
}
