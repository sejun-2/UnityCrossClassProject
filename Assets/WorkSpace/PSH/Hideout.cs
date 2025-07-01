using UnityEngine;

public class Hideout : MonoBehaviour,IInteractable
{
    private HideCheker _hideChecker;
    public void Interact()
    {
        if (_hideChecker != null)
        {
            if (_hideChecker.CanHide())
            {
                Debug.Log("은신 성공");
                // 은신 로직 실행
                Manager.Player.Stats.isHiding = true;
            }
            else
            {
                Debug.Log("추격 중인 좀비가 가까워서 은신 실패");
                // 은신 실패 처리
            }
        }
    }
    
}
