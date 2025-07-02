using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDisappear : MonoBehaviour
{
    [SerializeField] private float delay = .1f; // ������������ ������

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
        gameObject.SetActive(false); // ��Ȱ��ȭ
        // Destroy(gameObject); // �����ϰ� �ʹٸ� ��� ���
    }
}
