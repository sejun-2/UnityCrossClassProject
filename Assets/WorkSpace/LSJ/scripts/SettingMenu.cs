using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : BaseUI
{
    private void Start()
    {
        Debug.Log(GetEvent("SettingMenu����")); // SettingMenu UI�� ���۵� �� �α׸� ����մϴ�.
        // SelectButton ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
        GetEvent("SelectButton").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>(); // LangguagePopUp�� ǥ���մϴ�.
        GetEvent("SelectText").Enter += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.UI.PopUp.ShowPopUp<LangguagePopUp>();
        }
    }
}

