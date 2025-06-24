using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool _isOpen = false;

    GameObject _door;

    private void Start()
    {
        _door = gameObject;
    }
    public void Interact()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            Debug.Log("�� ����");
            //�� ���� �� ������ �ڵ�
            _door.SetActive(false);

        }
        else
        {
            Debug.Log("�� ����");
            //�� ���� �� ������ �ڵ�
            _door.SetActive(true);
        }
    }
}
