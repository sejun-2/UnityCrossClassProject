using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuPopUp : BaseUI
{
    int selectedIndex = 0;  // ���� ���õ� ��ư�� �ε���
    Button[] menuButtons;   // �޴� ��ư���� ������ �迭
    private bool inputEnabled = true; // �⺻�� true

    [Header("�ܺ� �޴� ������Ʈ")]
    [SerializeField] private GameObject Setting; // Inspector���� Setting ������Ʈ ����

    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _moveSound;

    private void Start()
    {
        Manager.Sound.SfxPlay(_clickSound, Camera.main.transform);
        Debug.Log(GetEvent("TitlePopUp����")); // UI�� ���۵� �� �α׸� ����մϴ�.

        menuButtons = new Button[]
        {
            GetEvent("ContinueButton")?.GetComponent<Button>(),
            GetEvent("SettingButton")?.GetComponent<Button>(),
            GetEvent("GoToTitleButton")?.GetComponent<Button>(),
            GetEvent("GameEndButton")?.GetComponent<Button>()
        };

        // Debug -> null üũ
        //for (int i = 0; i < menuButtons.Length; i++)
        //{
        //    if (menuButtons[i] == null)
        //        Debug.LogError($"menuButtons[{i}]�� null�Դϴ�! ������Ʈ �̸�, Button ������Ʈ Ȯ�� �ʿ�.");
        //}

        // ZŰ�� �����ϱ� ����, ��ư Ŭ�� �̺�Ʈ�� �迭�� �߰�
        menuButtons[0].onClick.AddListener(Manager.UI.PopUp.ClosePopUp);
        menuButtons[1].onClick.AddListener(showSerttingPopUp);
        menuButtons[2].onClick.AddListener(ChageTitleScene);
        menuButtons[3].onClick.AddListener(showEndPopUp);

        // ù ��° ��ư ����
        if (menuButtons[0] != null)
            menuButtons[0].Select();
    }

    private void OnEnable()
    {
        Debug.Log("MainMenuPopUp OnEnable ȣ��!");
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

        // menuButtons �迭�� ����ְų� �������� �ʾҴٸ� �ƹ� �͵� ���� �ʰ� �Լ� ����
        if (menuButtons == null || menuButtons.Length == 0) return;

        // ���� ����Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // selectedIndex�� �ϳ� ���δ�. (0���� �۾����� �� ������ �ε����� ��ȯ)
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            // �ش� �ε����� ��ư�� null�� �ƴϸ�
            if (menuButtons[selectedIndex] != null)
                // �� ��ư�� ����(��Ŀ��) ���·� ����� (���̶���Ʈ ǥ��)
                menuButtons[selectedIndex].Select();

            Manager.Sound.SfxPlay(_moveSound, Camera.main.transform);
        }

        // �Ʒ��� ����Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // selectedIndex�� �ϳ� �ø���. (������ �ε������� Ŀ���� 0������ ��ȯ)
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            // �ش� �ε����� ��ư�� null�� �ƴϸ�
            if (menuButtons[selectedIndex] != null)
                // �� ��ư�� ����(��Ŀ��) ���·� ����� (���̶���Ʈ ǥ��)
                menuButtons[selectedIndex].Select();

            Manager.Sound.SfxPlay(_moveSound, Camera.main.transform);
        }

        // ZŰ�� ������ ��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.Sound.SfxPlay(_clickSound, Camera.main.transform);
            // ���� ���õ� ��ư�� null�� �ƴϸ�
            if (menuButtons[selectedIndex] != null)
                // �� ��ư�� onClick �̺�Ʈ(��, Ŭ�� ȿ��)�� �����Ѵ�
                menuButtons[selectedIndex].onClick.Invoke();

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Sound.SfxPlay(_clickSound, Camera.main.transform);
            Manager.UI.PopUp.ClosePopUp();
        }
    }

    public void showSerttingPopUp() // ZŰ�� ������ �� ȣ��Ǵ� �޼���
    {
        Manager.UI.PopUp.ShowPopUp<SettingPopUp>(); // SettingPopUp�� ǥ���մϴ�.

        // Setting ������Ʈ Ȱ��ȭ
        if (Setting != null)
            Setting.SetActive(true);

        gameObject.SetActive(false); // MainMenuPopUp ��Ȱ��ȭ
    }

    // ���� ���� �˾��� ǥ���ϴ� �޼���
    public void showEndPopUp()
    {
        Manager.UI.PopUp.ShowPopUp<EndPopUp>();
    }

    // Ÿ��Ʋ ������ �̵��ϴ� �޼���
    public void ChageTitleScene()
    {
        Manager.Player.Stats.IsControl.Value = false;
        SceneChanger.ChageScene(sceneName: "TitleScene");
    }

    // SettingPopUp���� ȣ��
    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu ����");
        // Setting ������Ʈ ��Ȱ��ȭ
        if (Setting != null)
            Setting.SetActive(false);

        // MainMenuPopUp �ٽ� Ȱ��ȭ
        gameObject.SetActive(true);

        // ù ��° ��ư�� ��Ŀ��
        if (menuButtons != null && menuButtons.Length > 0 && menuButtons[0] != null)
        {
            selectedIndex = 0;
            menuButtons[0].Select();
        }
    }

}
