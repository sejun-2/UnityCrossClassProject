using UnityEngine;

public class RoomVisible : MonoBehaviour
{
    private Collider collider;       // �渶�� Ʈ����
    [SerializeField] GameObject dark;         // �渶�� ��� ������Ʈ
    public string targetTag = "Interactable";

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dark.SetActive(false);
        }
    }
    //�� �� �湮�� ���� �ٽ� ��ο����� ���� ���� �ϴ� ������
}
