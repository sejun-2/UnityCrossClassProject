using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePopUp : BaseUI
{
    private void Start()
    {
        Debug.Log(GetEvent("TitlePopUp열림")); // UI가 시작될 때 로그를 출력합니다.
        // 버튼이 클릭되었을 때 호출되는 메서드
        GetEvent("Continue").Click += data => Manager.UI.PopUp.ClosePopUp();
        GetEvent("Preferences").Click += data => Manager.UI.PopUp.ShowPopUp<SettingPopUp>();
        GetEvent("Title").Click += data => SceneChanger.ChageScene(sceneName: "TitleScene");
        GetEvent("GameOver").Click += data => Manager.UI.PopUp.ShowPopUp<EndPopUp>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Manager.UI.PopUp.ShowPopUp<EndPopUp>();
        //}

        

    }
}
