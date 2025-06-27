using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : BaseUI
{
    private void Start()
    {
        Debug.Log(GetEvent("SettingMenu열림")); // SettingMenu UI가 시작될 때 로그를 출력합니다.
        // SelectButton 버튼이 클릭되었을 때 호출되는 메서드
        GetEvent("SelectButton").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>(); // LangguagePopUp을 표시합니다.
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

