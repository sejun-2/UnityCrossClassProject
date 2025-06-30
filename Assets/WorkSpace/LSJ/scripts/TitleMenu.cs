using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : BaseUI
{
    private void Start()
    {
        Debug.Log(GetEvent("TitleMenu열림")); // UI가 시작될 때 로그를 출력합니다.
        // 버튼이 클릭되었을 때 호출되는 메서드
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
