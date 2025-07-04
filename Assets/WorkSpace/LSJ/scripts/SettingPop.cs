using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingPop : BaseUI
{
    [Header("탭 버튼")]
    [SerializeField] private Button SoundButton;
    [SerializeField] private Button LanguageButton;

    [Header("패널")]
    [SerializeField] private GameObject SoundPanel;
    [SerializeField] private GameObject LanguagePanel;

    [Header("슬라이더")]
    [SerializeField] public Slider MasterVolume;
    [SerializeField] public Slider BgmVolume;
    [SerializeField] public Slider SfxVolume;

    [Header("사운드 패널 내 네비게이션")]
    [SerializeField] private Selectable[] SoundSelectables; // MasterVolume, BgmVolume, SfxVolume 등
    [Header("언어 패널 내 네비게이션")]
    [SerializeField] private Selectable[] LanguageSelectables; // 언어 관련 버튼 등


    int selectedIndex = 0;  // 현재 선택된 버튼의 인덱스
    private Selectable[] currentSelectables;
    //Selectable[] menus;   // 메뉴 버튼들을 저장할 배열

    [SerializeField] public GameObject TitleMenu; //  UI 오브젝트

    private void Start()
    {
        Debug.Log(GetEvent("SettingMenu열림")); // SettingMenu UI가 시작될 때 로그를 출력합니다.
        // SelectButton 버튼이 클릭되었을 때 호출되는 메서드
        GetEvent("SelectButton").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>(); // LangguagePopUp을 표시합니다.
        GetEvent("SelectText").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>();

        GetEvent("ESCButton").Click += data => Manager.UI.PopUp.ClosePopUp();
        GetEvent("ESCText").Click += data => Manager.UI.PopUp.ClosePopUp();

        // 탭 버튼 클릭 이벤트 연결 (마우스 클릭용, 키보드는 Z키로 처리)
        SoundButton.onClick.AddListener(() => SwitchTab(0));
        LanguageButton.onClick.AddListener(() => SwitchTab(1));

        // 최초 사운드 탭 활성화
        SwitchTab(0);
    }

    private void SwitchTab(int tabIndex)
    {
        bool isSound = tabIndex == 0;
        SoundPanel.SetActive(isSound);
        LanguagePanel.SetActive(!isSound);

        // 탭 버튼 하이라이트(선택) 효과
        SoundButton.interactable = !isSound;
        LanguageButton.interactable = isSound;

        // 현재 선택 가능한 UI 배열 교체
        currentSelectables = isSound ? SoundSelectables : LanguageSelectables;
        selectedIndex = 0;
        if (currentSelectables != null && currentSelectables.Length > 0 && currentSelectables[0] != null)
        {
            EventSystem.current.SetSelectedGameObject(currentSelectables[0].gameObject);
        }
    }

    private void Update()
    {
        // 현재 선택 가능한 UI 요소가 없으면 아무 작업도 하지 않음
        if (currentSelectables == null || currentSelectables.Length == 0) return;

        // 위/아래 방향키로 선택 이동
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + currentSelectables.Length) % currentSelectables.Length;
            SelectCurrent();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % currentSelectables.Length;
            SelectCurrent();
        }

        // 좌/우 방향키는 슬라이더에서만 값 조절 용도로 사용 (탭 전환 금지)
        // 탭 전환은 오직 Z키로 SoundButton/LanguageButton을 눌렀을 때만!

        // Z키로 현재 선택된 버튼 클릭(버튼만)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var sel = currentSelectables[selectedIndex];
            if (sel is Button btn)
            {
                if (btn == SoundButton)
                {
                    SwitchTab(0);
                }
                else if (btn == LanguageButton)
                {
                    SwitchTab(1);
                }
                else
                {
                    btn.onClick.Invoke();
                }
            }
        }

        // ESC키로 팝업 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TitleMenu.SetActive(true);
            gameObject.SetActive(false);

        }

        // 사운드 값 실시간 반영
        if (MasterVolume != null && BgmVolume != null && SfxVolume != null && Manager.Sound != null)
        {
            Manager.Sound.MasterVolume = MasterVolume.value;
            Manager.Sound.BgmVolume = BgmVolume.value;
            Manager.Sound.SfxVolume = SfxVolume.value;
        }
    }

    private void SelectCurrent()
    {
        if (currentSelectables[selectedIndex] != null)
            EventSystem.current.SetSelectedGameObject(currentSelectables[selectedIndex].gameObject);
    }

    private void OnDisable()
    { 
        if (TitleMenu != null)
        {
            Debug.Log("SettingPopUp OnDisable 호출됨");
            TitleMenu.SetActive(true);
        }

    }


}

