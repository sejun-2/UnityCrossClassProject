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
                Debug.LogError("Player ������Ʈ�� HideChecker ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
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
                Debug.Log("���� ����");
                // ���� ���� ����
                Manager.Player.Stats.IsHiding = true;
                return true;
            }
            else
            {
                Debug.Log("�߰� ���� ���� ������� ���� ����");
                // ���� ���� ó��
                return false;
            }
        }

        return false;
    }
}
