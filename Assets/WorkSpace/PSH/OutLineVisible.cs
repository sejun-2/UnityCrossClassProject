using EPOOutline;
using UnityEngine;

public class OutLineVisible : MonoBehaviour
{
    //오브젝트의 상태에 따라 윤곽선을 표시하는 스크립트
    //방 탐색 여부에 따라 실루엣 표시

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
