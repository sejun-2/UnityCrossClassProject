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
                Debug.LogError("Player ������Ʈ�� HideChecker ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
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
                Debug.Log("���� ����");
                box.gameObject.SetActive(true);
                // �ڽ��� �÷��̾� ���� �α�
                box.transform.position = playerTransform.position + Vector3.up * 3;

                // �ڽ��� �Ʒ��� �������� DOTween
                MoveWithDOTween(box.transform, playerTransform.position + Vector3.up * 1.4f, .5f);

                Manager.Player.Stats.IsHiding = true;
                return true;
            }
            else
            {
                Debug.Log("�߰� ���� ���� ������� ���� ����");
                return false;
            }
        }
        return false;
    }

    public void LiftBoxOffPlayer(Transform playerTransform)
    {
        // �ڽ��� �ٽ� ���� �ø���
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
