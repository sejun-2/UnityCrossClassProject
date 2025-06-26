using UnityEngine;

public class OutLineVisible : MonoBehaviour
{
    //������Ʈ�� ���¿� ���� �������� ǥ���ϴ� ��ũ��Ʈ

    private Renderer _renderer;
    private Material _mat;

    [SerializeField] private float onScale = 1.2f;
    [SerializeField] private float offScale = 0f;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null && _renderer.materials.Length > 1)
        {
            _mat = _renderer.materials[1]; // �� ��° ���׸����� �ƿ�����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _mat != null)
        {
            _mat.SetFloat("_Scale", onScale);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _mat != null)
        {
            _mat.SetFloat("_Scale", offScale);
        }
    }
}
