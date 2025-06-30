using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : BaseUI
{
    private void Start()
    {
        Debug.Log(GetEvent("TitleMenu����")); // UI�� ���۵� �� �α׸� ����մϴ�.
        // ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
        GetEvent("Preferences").Click += data => Manager.UI.PopUp.ShowPopUp<TitlePopUp>();
        GetEvent("Preferences").Enter += data => Manager.UI.PopUp.ShowPopUp<TitlePopUp>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.UI.PopUp.ShowPopUp<TitlePopUp>();
        }
    }
}
