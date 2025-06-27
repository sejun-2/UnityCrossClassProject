using UnityEngine;

public class OutLineVisible : MonoBehaviour
{
    //오브젝트의 상태에 따라 윤곽선을 표시하는 스크립트
    //방 탐색 여부에 따라 실루엣 표시

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
            _outlineMat = _renderer.materials[1]; // 두 번째 머테리얼이 아웃라인
            _silhouetteMat = _renderer.materials[2]; // 세 번째 머테리얼이 실루엣
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
