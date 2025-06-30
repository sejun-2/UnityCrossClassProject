using EPOOutline;
using UnityEngine;

public class OutLineVisible : MonoBehaviour
{
    //������Ʈ�� ���¿� ���� �������� ǥ���ϴ� ��ũ��Ʈ
    //�� Ž�� ���ο� ���� �Ƿ翧 ǥ��

    private Outlinable _outlinable;
    private Color _color;
    private bool _silhouetteShown = true;

    [SerializeField] private GameObject linkedDarkness;



    private void Awake()
    {
        _outlinable = GetComponentInChildren<Outlinable>();
        if (linkedDarkness == null)
            _outlinable.enabled = false;
        _color = _outlinable.FrontParameters.Color;

        if (linkedDarkness == null)
            SetSilhouetteVisible();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _outlinable.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _outlinable.enabled = false;
        }
    }
    private void Update()
    {
        if (linkedDarkness != null && _silhouetteShown)
            if (!linkedDarkness.activeSelf)
            {
                SetSilhouetteVisible();
                _silhouetteShown = false;
            }
    }
    public void SetSilhouetteVisible()
    {
        _outlinable.BackParameters.Color = _color;
        _outlinable.enabled = false;
    }
}
