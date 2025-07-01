using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUp : BaseUI
{
    [SerializeField] private Slider MasterVolume;
    [SerializeField] private Slider BgmVolume;
    [SerializeField] private Slider SfxVolume;

    int selectedIndex = 0;  // ���� ���õ� ��ư�� �ε���
    Selectable[] menus;   // �޴� ��ư���� ������ �迭

    private void Start()
    {
        Debug.Log(GetEvent("SettingMenu����")); // SettingMenu UI�� ���۵� �� �α׸� ����մϴ�.
        // SelectButton ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
        GetEvent("SelectButton").Click += data => Manager.UI.PopUp.ShowPopUp<LangguagePopUp>(); // LangguagePopUp�� ǥ���մϴ�.
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
                Debug.LogError($"menus[{i}]�� null�Դϴ�! ������Ʈ �̸�, Button ������Ʈ Ȯ�� �ʿ�.");
        }

        // ù ��° ��ư ����
        if (menus[0] != null)
            menus[0].Select();
    }

    private void Update()
    {
        // menuButtons �迭�� ����ְų� �������� �ʾҴٸ� �ƹ� �͵� ���� �ʰ� �Լ� ����
        if (menus == null || menus.Length == 0) return;

        // ���� ����Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // selectedIndex�� �ϳ� ���δ�. (0���� �۾����� �� ������ �ε����� ��ȯ)
            selectedIndex = (selectedIndex - 1 + menus.Length) % menus.Length;
            // �ش� �ε����� ��ư�� null�� �ƴϸ�
            if (menus[selectedIndex] != null)
                // �� ��ư�� ����(��Ŀ��) ���·� ����� (���̶���Ʈ ǥ��)
                menus[selectedIndex].Select();
        }

        // �Ʒ��� ����Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // selectedIndex�� �ϳ� �ø���. (������ �ε������� Ŀ���� 0������ ��ȯ)
            selectedIndex = (selectedIndex + 1) % menus.Length;
            // �ش� �ε����� ��ư�� null�� �ƴϸ�
            if (menus[selectedIndex] != null)
                // �� ��ư�� ����(��Ŀ��) ���·� ����� (���̶���Ʈ ǥ��)
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

        Manager.Sound.MasterVolume = MasterVolume.value; // MasterVolume �����̴��� ���� SoundManager�� MasterVolume�� �Ҵ��մϴ�.
        Manager.Sound.BgmVolume = BgmVolume.value; // BgmVolume �����̴��� ���� SoundManager�� BgmVolume�� �Ҵ��մϴ�.
        Manager.Sound.SfxVolume = SfxVolume.value; // SfxVolume �����̴��� ���� SoundManager�� SfxVolume�� �Ҵ��մϴ�.

    }
}

