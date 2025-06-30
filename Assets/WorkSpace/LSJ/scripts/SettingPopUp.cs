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
        Debug.Log(GetEvent("SettingMenu����")); // SettingMenu UI�� ���۵� �� �α׸� ����մϴ�.
        // SelectButton ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
        GetEvent("SelectButton").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>(); // LangguagePopUp�� ǥ���մϴ�.
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

        Manager.Sound.MasterVolume = MasterVolume.value; // MasterVolume �����̴��� ���� SoundManager�� MasterVolume�� �Ҵ��մϴ�.
        Manager.Sound.BgmVolume = BgmVolume.value; // BgmVolume �����̴��� ���� SoundManager�� BgmVolume�� �Ҵ��մϴ�.
        Manager.Sound.SfxVolume = SfxVolume.value; // SfxVolume �����̴��� ���� SoundManager�� SfxVolume�� �Ҵ��մϴ�.

    }
}

