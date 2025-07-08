using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuPopUp : BaseUI
{
    int selectedIndex = 0;  // 현재 선택된 버튼의 인덱스
    Button[] menuButtons;   // 메뉴 버튼들을 저장할 배열
    private bool inputEnabled = true; // 기본값 true

    [Header("외부 메뉴 오브젝트")]
    [SerializeField] private GameObject Setting; // Inspector에서 Setting 오브젝트 연결

    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _moveSound;

    private void Start()
    {
        Manager.Sound.SfxPlay(_clickSound, Camera.main.transform);
        Debug.Log(GetEvent("TitlePopUp열림")); // UI가 시작될 때 로그를 출력합니다.

        menuButtons = new Button[]
        {
            GetEvent("ContinueButton")?.GetComponent<Button>(),
            GetEvent("SettingButton")?.GetComponent<Button>(),
            GetEvent("GoToTitleButton")?.GetComponent<Button>(),
            GetEvent("GameEndButton")?.GetComponent<Button>()
        };

        // Debug -> null 체크
        //for (int i = 0; i < menuButtons.Length; i++)
        //{
        //    if (menuButtons[i] == null)
        //        Debug.LogError($"menuButtons[{i}]가 null입니다! 오브젝트 이름, Button 컴포넌트 확인 필요.");
        //}

        // Z키로 실행하기 위해, 버튼 클릭 이벤트를 배열에 추가
        menuButtons[0].onClick.AddListener(Manager.UI.PopUp.ClosePopUp);
        menuButtons[1].onClick.AddListener(showSerttingPopUp);
        menuButtons[2].onClick.AddListener(ChageTitleScene);
        menuButtons[3].onClick.AddListener(showEndPopUp);

        // 첫 번째 버튼 선택
        if (menuButtons[0] != null)
            menuButtons[0].Select();
    }

    private void OnEnable()
    {
        Debug.Log("MainMenuPopUp OnEnable 호출!");
        inputEnabled = true;
        if (menuButtons != null && menuButtons.Length > 0 && menuButtons[0] != null)
        {
            selectedIndex = 0;
            menuButtons[0].Select();
        }

    }

    private void OnDisable()
    {
        inputEnabled = false;
    }

    private void Update()
    {
        if (!inputEnabled) return;

        // menuButtons 배열이 비어있거나 생성되지 않았다면 아무 것도 하지 않고 함수 종료
        if (menuButtons == null || menuButtons.Length == 0) return;

        // 위쪽 방향키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // selectedIndex를 하나 줄인다. (0보다 작아지면 맨 마지막 인덱스로 순환)
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            // 해당 인덱스의 버튼이 null이 아니면
            if (menuButtons[selectedIndex] != null)
                // 그 버튼을 선택(포커스) 상태로 만든다 (하이라이트 표시)
                menuButtons[selectedIndex].Select();

            Manager.Sound.SfxPlay(_moveSound, Camera.main.transform);
        }

        // 아래쪽 방향키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // selectedIndex를 하나 늘린다. (마지막 인덱스보다 커지면 0번으로 순환)
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            // 해당 인덱스의 버튼이 null이 아니면
            if (menuButtons[selectedIndex] != null)
                // 그 버튼을 선택(포커스) 상태로 만든다 (하이라이트 표시)
                menuButtons[selectedIndex].Select();

            Manager.Sound.SfxPlay(_moveSound, Camera.main.transform);
        }

        // Z키가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.Sound.SfxPlay(_clickSound, Camera.main.transform);
            // 현재 선택된 버튼이 null이 아니면
            if (menuButtons[selectedIndex] != null)
                // 그 버튼의 onClick 이벤트(즉, 클릭 효과)를 실행한다
                menuButtons[selectedIndex].onClick.Invoke();

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Sound.SfxPlay(_clickSound, Camera.main.transform);
            Manager.UI.PopUp.ClosePopUp();
        }
    }

    public void showSerttingPopUp() // Z키를 눌렀을 때 호출되는 메서드
    {
        Manager.UI.PopUp.ShowPopUp<SettingPopUp>(); // SettingPopUp을 표시합니다.

        // Setting 오브젝트 활성화
        if (Setting != null)
            Setting.SetActive(true);

        gameObject.SetActive(false); // MainMenuPopUp 비활성화
    }

    // 게임 종료 팝업을 표시하는 메서드
    public void showEndPopUp()
    {
        Manager.UI.PopUp.ShowPopUp<EndPopUp>();
    }

    // 타이틀 씬으로 이동하는 메서드
    public void ChageTitleScene()
    {
        Manager.Player.Stats.IsControl.Value = false;
        SceneChanger.ChageScene(sceneName: "TitleScene");
    }

    // SettingPopUp에서 호출
    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu 실행");
        // Setting 오브젝트 비활성화
        if (Setting != null)
            Setting.SetActive(false);

        // MainMenuPopUp 다시 활성화
        gameObject.SetActive(true);

        // 첫 번째 버튼에 포커스
        if (menuButtons != null && menuButtons.Length > 0 && menuButtons[0] != null)
        {
            selectedIndex = 0;
            menuButtons[0].Select();
        }
    }

}
