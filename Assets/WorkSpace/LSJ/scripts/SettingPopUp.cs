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

        //Manager.Player.Stats.Volume.OnChanged += (newVolume) => Volume.value = newVolume;
    }
}

