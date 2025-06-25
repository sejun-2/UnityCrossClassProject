using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject linkedDarkness;

    private bool _isOpen = false;

    private Transform _meshTransform;
    private Vector3 _closedPos;
    [SerializeField] private float openDistance = 6f;

    private void Start()
    {
        // 자식 중 "Mesh" 이름의 오브젝트 찾기
        _meshTransform = transform.Find("Mesh");

        if (_meshTransform == null)
        {
            Debug.Log("Mesh 오브젝트를 찾을 수 없습니다.");
            return;
        }

        _closedPos = _meshTransform.localPosition;
    }
    public void Interact()
    {
        if (_meshTransform == null) return;

        _isOpen = !_isOpen;

        if (_isOpen)
        {
            Debug.Log("문 열림");
            _meshTransform.localPosition = _closedPos + new Vector3(0f, 0f, openDistance);

            if (linkedDarkness != null)
                linkedDarkness.SetActive(false);
        }
        else
        {
            Debug.Log("문 닫힘");
            _meshTransform.localPosition = _closedPos;

            if (linkedDarkness != null)
                linkedDarkness.SetActive(true);
        }
    }
}
