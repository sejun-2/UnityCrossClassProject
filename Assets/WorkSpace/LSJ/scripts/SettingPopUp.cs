using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUp : BaseUI
{
    [SerializeField] private Slider Volume;
    [SerializeField] private Slider SoundEffects;
    [SerializeField] private Slider BackgroundMusic;

    private void Start()
    {
        Debug.Log(GetEvent("SettingMenu열림")); // SettingMenu UI가 시작될 때 로그를 출력합니다.
        // SelectButton 버튼이 클릭되었을 때 호출되는 메서드
        GetEvent("SelectButton").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>(); // LangguagePopUp을 표시합니다.
        GetEvent("SelectText").Enter += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>();

        GetEvent("ESCButton").Click += data => Manager.UI.PopUp.ClosePopUp();
        GetEvent("ESCText").Click += data => Manager.UI.PopUp.ClosePopUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.UI.PopUp.ShowPopUp<LangguagePopUp>();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UI.PopUp.ClosePopUp();
        }

        //Manager.Player.Stats.Volume.OnChanged += (newVolume) => Volume.value = newVolume;
    }
}

