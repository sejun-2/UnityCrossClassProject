using UnityEngine;

public class OutLineVisible : MonoBehaviour
{
    //������Ʈ�� ���¿� ���� �������� ǥ���ϴ� ��ũ��Ʈ
    //�� Ž�� ���ο� ���� �Ƿ翧 ǥ��

    private Renderer _renderer;
    private Material _outlineMat;
    private Material _silhouetteMat;
    [SerializeField] private GameObject linkedDarkness;

    [SerializeField] private float onScale = 1.2f;
    [SerializeField] private float offScale = 0f;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null && _renderer.materials.Length > 1)
        {
            _outlineMat = _renderer.materials[1]; // �� ��° ���׸����� �ƿ�����
            _silhouetteMat = _renderer.materials[2]; // �� ��° ���׸����� �Ƿ翧
        }

        if (linkedDarkness == null)
            Destroy(_silhouetteMat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _outlineMat != null)
        {
            _outlineMat.SetFloat("_Scale", onScale);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _outlineMat != null)
        {
            _outlineMat.SetFloat("_Scale", offScale);
        }
    }
    private void Update()
    {
        if (linkedDarkness != null)
            if (!linkedDarkness.activeSelf)
                SetSilhouetteVisible();
    }
    public void SetSilhouetteVisible()
    {
        if (_silhouetteMat != null)
        {
            _silhouetteMat.SetFloat("_Alpha", 0f);
        }
    }
}
