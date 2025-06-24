using UnityEngine;

public class RoomVisible : MonoBehaviour
{
    [SerializeField] Collider collider;       // 방마다 트리거
    [SerializeField] GameObject dark;         // 방마다 어둠 오브젝트

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(dark);
        }
    }
    //한 번 방문한 방은 다시 어두워지지 않음 으로 일단 구현함
}
