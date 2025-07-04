using DG.Tweening;
using UnityEngine;

public class Hideout : MonoBehaviour,IInteractable
{
    private HideChecker _hideChecker;

    [SerializeField] GameObject box;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            _hideChecker = playerObj.GetComponent<HideChecker>();

            if (_hideChecker == null)
            {
                Debug.LogError("Player 오브젝트에 HideChecker 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        box.gameObject.SetActive(false);
    }
    public void Interact()
    {
       
    }
    public bool Interact(Transform playerTransform)
    {
        if (_hideChecker != null)
        {
            if (_hideChecker.CanHide())
            {
                Debug.Log("은신 성공");
                box.gameObject.SetActive(true);
                // 박스를 플레이어 위에 두기
                box.transform.position = playerTransform.position + Vector3.up * 3;

                // 박스를 아래로 내려놓는 DOTween
                MoveWithDOTween(box.transform, playerTransform.position + Vector3.up * 1.4f, .5f);

                Manager.Player.Stats.IsHiding = true;
                return true;
            }
            else
            {
                Debug.Log("추격 중인 좀비가 가까워서 은신 실패");
                return false;
            }
        }
        return false;
    }

    public void LiftBoxOffPlayer(Transform playerTransform)
    {
        // 박스를 다시 위로 올리기
        MoveWithDOTween(box.transform, playerTransform.position + Vector3.up * 3, .5f, true);
    }

    void MoveWithDOTween(Transform target, Vector3 endPos, float duration, bool deactivateOnComplete = false)
    {
        var tween = target.DOMove(endPos, duration)
                          .SetEase(Ease.InOutSine);

        if (deactivateOnComplete)
        {
            tween.OnComplete(() => target.gameObject.SetActive(false));
        }
    }
}
