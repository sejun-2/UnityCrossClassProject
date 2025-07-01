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
                Debug.Log("���� ����");
                // ���� ���� ����
                Manager.Player.Stats.isHiding = true;
            }
            else
            {
                Debug.Log("�߰� ���� ���� ������� ���� ����");
                // ���� ���� ó��
            }
        }
    }
    
}
