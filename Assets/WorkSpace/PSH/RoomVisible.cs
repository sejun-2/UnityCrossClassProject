using UnityEngine;

public class RoomVisible : MonoBehaviour
{
    [SerializeField] Collider collider;       // �渶�� Ʈ����
    [SerializeField] GameObject dark;         // �渶�� ��� ������Ʈ

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
    //�� �� �湮�� ���� �ٽ� ��ο����� ���� ���� �ϴ� ������
}
