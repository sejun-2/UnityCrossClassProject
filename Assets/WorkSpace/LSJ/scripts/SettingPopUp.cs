using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUp : BaseUI
{
    [SerializeField] private Slider MasterVolume;
    [SerializeField] private Slider BgmVolume;
    [SerializeField] private Slider SfxVolume;

    int selectedIndex = 0;  // 현재 선택된 버튼의 인덱스
    Selectable[] menus;   // 메뉴 버튼들을 저장할 배열

    private void Start()
    {
        Debug.Log(GetEvent("SettingMenu열림")); // SettingMenu UI가 시작될 때 로그를 출력합니다.
        // SelectButton 버튼이 클릭되었을 때 호출되는 메서드
        GetEvent("SelectButton").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>(); // LangguagePopUp을 표시합니다.
        GetEvent("SelectText").Enter += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>();

        GetEvent("ESCButton").Click += data => Manager.UI.PopUp.ClosePopUp();
        GetEvent("ESCText").Click += data => Manager.UI.PopUp.ClosePopUp();

        menus = new Selectable[]
        {
            GetEvent("SoundButton")?.GetComponent<Selectable>(),
            GetEvent("LanguageButton")?.GetComponent<Selectable>(),
            GetEvent("MasterVolume")?.GetComponent<Selectable>(),
            GetEvent("BgmVolume")?.GetComponent<Selectable>(),
            GetEvent("SfxVolume")?.GetComponent<Selectable>(),
        };

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i] == null)
                Debug.LogError($"menus[{i}]가 null입니다! 오브젝트 이름, Button 컴포넌트 확인 필요.");
        }

        // 첫 번째 버튼 선택
        if (menus[0] != null)
            menus[0].Select();
    }

    private void Update()
    {
        // menuButtons 배열이 비어있거나 생성되지 않았다면 아무 것도 하지 않고 함수 종료
        if (menus == null || menus.Length == 0) return;

        // 위쪽 방향키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // selectedIndex를 하나 줄인다. (0보다 작아지면 맨 마지막 인덱스로 순환)
            selectedIndex = (selectedIndex - 1 + menus.Length) % menus.Length;
            // 해당 인덱스의 버튼이 null이 아니면
            if (menus[selectedIndex] != null)
                // 그 버튼을 선택(포커스) 상태로 만든다 (하이라이트 표시)
                menus[selectedIndex].Select();
        }

        // 아래쪽 방향키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // selectedIndex를 하나 늘린다. (마지막 인덱스보다 커지면 0번으로 순환)
            selectedIndex = (selectedIndex + 1) % menus.Length;
            // 해당 인덱스의 버튼이 null이 아니면
            if (menus[selectedIndex] != null)
                // 그 버튼을 선택(포커스) 상태로 만든다 (하이라이트 표시)
                menus[selectedIndex].Select();
        }

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

