using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePopUp : BaseUI
{
    int selectedIndex = 0;  // ���� ���õ� ��ư�� �ε���
    Button[] menuButtons;   // �޴� ��ư���� ������ �迭
    private bool inputEnabled = false;

    private void Start()
    {
        Debug.Log(GetEvent("TitlePopUp����")); // UI�� ���۵� �� �α׸� ����մϴ�.

        menuButtons = new Button[]
        {
            GetEvent("Continue")?.GetComponent<Button>(),
            GetEvent("Preferences")?.GetComponent<Button>(),
            GetEvent("Title")?.GetComponent<Button>(),
            GetEvent("GameOver")?.GetComponent<Button>()
        };

        // null üũ
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i] == null)
                Debug.LogError($"menuButtons[{i}]�� null�Դϴ�! ������Ʈ �̸�, Button ������Ʈ Ȯ�� �ʿ�.");
        }

        // �̺�Ʈ ����, ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
        GetEvent("Continue").Click += data => Manager.UI.PopUp.ClosePopUp();
        GetEvent("Preferences").Click += data => Manager.UI.PopUp.ShowPopUp<SettingPopUp>();
        GetEvent("Title").Click += data => SceneChanger.ChageScene(sceneName: "TitleScene");
        GetEvent("GameOver").Click += data => Manager.UI.PopUp.ShowPopUp<EndPopUp>();

        // ZŰ�� �����ϱ� ����, ��ư Ŭ�� �̺�Ʈ�� �迭�� �߰�
        menuButtons[0].onClick.AddListener(Manager.UI.PopUp.ClosePopUp);
        menuButtons[1].onClick.AddListener(showSerttingPopUp);
        menuButtons[2].onClick.AddListener(ChageTitleScene);
        menuButtons[3].onClick.AddListener(showEndPopUp);

        // ù ��° ��ư ����
        if (menuButtons[0] != null)
            menuButtons[0].Select();
    }

    private void Update()
    {
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
        }

        // ZŰ�� ������ ��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // ���� ���õ� ��ư�� null�� �ƴϸ�
            if (menuButtons[selectedIndex] != null)
                // �� ��ư�� onClick �̺�Ʈ(��, Ŭ�� ȿ��)�� �����Ѵ�
                menuButtons[selectedIndex].onClick.Invoke();
        }
    }

    public void showSerttingPopUp() // ZŰ�� ������ �� ȣ��Ǵ� �޼���
    {
        Manager.UI.PopUp.ShowPopUp<SettingPopUp>(); // SettingPopUp�� ǥ���մϴ�.
    }

    public void showEndPopUp()
    {
        Manager.UI.PopUp.ShowPopUp<EndPopUp>(); // EndPopUp�� ǥ���մϴ�.
    }
    public void ChageTitleScene()
    {
        SceneChanger.ChageScene(sceneName: "TitleScene");   // TitleScene���� �����մϴ�.
    }
}
