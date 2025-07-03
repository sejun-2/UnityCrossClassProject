using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDisappear : MonoBehaviour
{
    [SerializeField] private float delay = .1f; // 사라지기까지의 딜레이

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
        gameObject.SetActive(false); // 비활성화
        // Destroy(gameObject); // 삭제하고 싶다면 대신 사용
    }
}
