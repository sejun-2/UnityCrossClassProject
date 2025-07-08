using EPOOutline;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutLineVisible : MonoBehaviour
{
    //오브젝트의 상태에 따라 윤곽선을 표시하는 스크립트
    //방 탐색 여부에 따라 실루엣 표시

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

    public void SetSilhouetteInvisible()//2색을 1색으로
    {
        _outlinable.BackParameters.Color = _frontColor;
        _outlinable.enabled = false;
    }

    public void SetSilhouetteVisible()//1색을 2색으로
    {
        _outlinable.BackParameters.Color = _backColor;
        _outlinable.enabled = true;
    }
}
