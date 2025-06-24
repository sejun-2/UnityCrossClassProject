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
            Debug.Log("문 열림");
            //문 열릴 때 실행할 코드
            _door.SetActive(false);

        }
        else
        {
            Debug.Log("문 닫힘");
            //문 닫힐 때 실행할 코드
            _door.SetActive(true);
        }
    }
}
