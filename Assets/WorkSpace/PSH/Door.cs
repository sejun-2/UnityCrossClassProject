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
        // �ڽ� �� "Mesh" �̸��� ������Ʈ ã��
        _meshTransform = transform.Find("Mesh");

        if (_meshTransform == null)
        {
            Debug.Log("Mesh ������Ʈ�� ã�� �� �����ϴ�.");
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
            Debug.Log("�� ����");
            _meshTransform.localPosition = _closedPos + new Vector3(0f, 0f, openDistance);

            if (linkedDarkness != null)
                linkedDarkness.SetActive(false);
        }
        else
        {
            Debug.Log("�� ����");
            _meshTransform.localPosition = _closedPos;

            if (linkedDarkness != null)
                linkedDarkness.SetActive(true);
        }
    }
}
