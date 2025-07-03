using EPOOutline;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutLineVisible : MonoBehaviour
{
    //������Ʈ�� ���¿� ���� �������� ǥ���ϴ� ��ũ��Ʈ
    //�� Ž�� ���ο� ���� �Ƿ翧 ǥ��

    private Outlinable _outlinable;
    private Color _color;
    private bool _silhouetteShown = true;



    private void Awake()
    {
        _outlinable = GetComponentInChildren<Outlinable>();
        _color = _outlinable.FrontParameters.Color;
    }

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        //���̽�ķ�������� �̷��� �ض�
        if (currentScene == "BaseCamp")
        {
            SetSilhouetteVisible();
        }
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

    public void SetSilhouetteVisible()
    {
        _outlinable.BackParameters.Color = _color;
        _outlinable.enabled = false;
    }
}
