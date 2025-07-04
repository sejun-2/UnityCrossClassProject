using UnityEngine;

public class Hideout : MonoBehaviour,IInteractable
{
    private HideChecker _hideChecker;

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
    }
    public void Interact()
    {
       
    }
    public bool Interact(int a)
    {
        if (_hideChecker != null)
        {
            if (_hideChecker.CanHide())
            {
                Debug.Log("은신 성공");
                // 은신 로직 실행
                Manager.Player.Stats.IsHiding = true;
                return true;
            }
            else
            {
                Debug.Log("추격 중인 좀비가 가까워서 은신 실패");
                // 은신 실패 처리
                return false;
            }
        }

        return false;
    }
}
