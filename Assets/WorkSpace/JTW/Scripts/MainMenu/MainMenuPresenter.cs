using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPresenter : BaseUI
{
    [SerializeField] private ItemSlotUIs _mainSlotUIsPrefab;
    [SerializeField] private AudioClip _bgmClip;

    private GameObject _slotPanel;

    private ItemSlotUIs _slotUIs;

    private GameObject _popUp;

    private readonly string[] _mainText = new string[] { "게임 시작", "환경 설정", "게임 종료" };

    private void Start()
    {
        _slotPanel = GetUI("SlotPanel");

        _slotUIs = Instantiate(_mainSlotUIsPrefab, _slotPanel.transform).GetComponent<ItemSlotUIs>();
        _slotUIs.SetLineCount(1);
        _slotUIs.SetPanelSize(new Vector2(1, 3));

        for(int i = 0; i < 3; i++)
        {
            _slotUIs.AddSlotUI(maxItemCount: 0);

            _slotUIs.SlotUIs[i].SetText(_mainText[i]);
        }

        _slotUIs.SelectSlotUI(0);
        _slotUIs.SlotUIs[_slotUIs.SelectedSlotIndex].SetColor(Color.yellow);

        Manager.Sound.BgmPlay(_bgmClip);

        if (Manager.Game.IsSaved())
        {
            _slotUIs.SlotUIs[0].SetText("계속하기");
        }
    }

    private void Update()
    {
        if (_popUp != null) return;

        MoveSlot();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(_slotUIs.SelectedSlotIndex == 0)
            {
                Manager.Game.GameStart();
            }
            else if(_slotUIs.SelectedSlotIndex == 1)
            {
                _popUp = Manager.UI.PopUp.ShowPopUp<MainMenuPopUp>().gameObject;
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

    }
    private void MoveSlot()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelectSlot(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelectSlot(Vector2.left);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelectSlot(Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelectSlot(Vector2.down);
        }
    }

    private void ChangeSelectSlot(Vector2 direction)
    {
        _slotUIs.SlotUIs[_slotUIs.SelectedSlotIndex].SetColor(Color.white);
        _slotUIs.MoveSelectSlot(direction);
        _slotUIs.SlotUIs[_slotUIs.SelectedSlotIndex].SetColor(Color.yellow);

    }
}
