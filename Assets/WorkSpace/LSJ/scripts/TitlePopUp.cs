using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePopUp : BaseUI
{
    private void Start()
    {
        Debug.Log(GetEvent("TitlePopUp����")); // UI�� ���۵� �� �α׸� ����մϴ�.
        // ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
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
