using EPOOutline;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutLineVisible : MonoBehaviour
{
    //오브젝트의 상태에 따라 윤곽선을 표시하는 스크립트
    //방 탐색 여부에 따라 실루엣 표시

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

        //베이스캠프에서는 이렇게 해라
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
