using EPOOutline;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutLineVisible : MonoBehaviour
{
    //������Ʈ�� ���¿� ���� �������� ǥ���ϴ� ��ũ��Ʈ
    //�� Ž�� ���ο� ���� �Ƿ翧 ǥ��

    private Outlinable _outlinable;
    private Color _frontColor;
    private Color _backColor;


    private void Awake()
    {
        _outlinable = GetComponentInChildren<Outlinable>();
        _frontColor = _outlinable.FrontParameters.Color;
        _backColor = _outlinable.BackParameters.Color;
        SetSilhouetteInvisible();
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

    public void SetSilhouetteInvisible()//2���� 1������
    {
        _outlinable.BackParameters.Color = _frontColor;
        _outlinable.enabled = false;
    }

    public void SetSilhouetteVisible()//1���� 2������
    {
        _outlinable.BackParameters.Color = _backColor;
        _outlinable.enabled = true;
    }
}
