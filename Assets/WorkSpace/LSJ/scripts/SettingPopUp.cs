using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUp : BaseUI
{
    [SerializeField] private Slider MasterVolume;
    [SerializeField] private Slider BgmVolume;
    [SerializeField] private Slider SfxVolume;

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

        Manager.Sound.MasterVolume = MasterVolume.value; // MasterVolume 슬라이더의 값을 SoundManager의 MasterVolume에 할당합니다.
        Manager.Sound.BgmVolume = BgmVolume.value; // BgmVolume 슬라이더의 값을 SoundManager의 BgmVolume에 할당합니다.
        Manager.Sound.SfxVolume = SfxVolume.value; // SfxVolume 슬라이더의 값을 SoundManager의 SfxVolume에 할당합니다.

    }
}

