using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPresenter : BaseUI
{
    [SerializeField] private ItemSlotUIs _mainSlotUIsPrefab;
    [SerializeField] private AudioClip _bgmClip;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _moveSound;

    private GameObject _slotPanel;

    private ItemSlotUIs _slotUIs;

    private GameObject _popUp;

    private readonly string[] _mainText = new string[] { "���� ����", "ȯ�� ����", "���� ����" };

    private Coroutine _startCoroutine;

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

        Manager.Sound.BgmPlay(_bgmClip, 0.4f);

        if (Manager.Game.IsSaved())
        {
            _slotUIs.SlotUIs[0].SetText("����ϱ�");
        }
    }

    private void Update()
    {
        if (_popUp != null) return;
        if (_startCoroutine != null) return;

        MoveSlot();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(_slotUIs.SelectedSlotIndex == 0)
            {
                _startCoroutine = StartCoroutine(StartCor());
            }
            else if(_slotUIs.SelectedSlotIndex == 1)
            {
                _popUp = Manager.UI.PopUp.ShowPopUp<SettingPopUp>().gameObject;
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            Manager.Sound.SfxPlay(_clickSound, Camera.main.transform);
        }

    }

    private IEnumerator StartCor()
    {
        GetUI<Image>("FadeImage").DOFade(1f, 1f);
        yield return new WaitForSeconds(2f);

        Manager.Game.GameStart();
    }


    private void MoveSlot()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_slotUIs.SelectedSlotIndex <= 0) return;

            ChangeSelectSlot(Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_slotUIs.SelectedSlotIndex >= _slotUIs.SlotUIs.Count - 1) return;

            ChangeSelectSlot(Vector2.down);
        }
    }

    private void ChangeSelectSlot(Vector2 direction)
    {
        Manager.Sound.SfxPlay(_moveSound, Camera.main.transform);
        _slotUIs.SlotUIs[_slotUIs.SelectedSlotIndex].SetColor(Color.white);
        _slotUIs.MoveSelectSlot(direction);
        _slotUIs.SlotUIs[_slotUIs.SelectedSlotIndex].SetColor(Color.yellow);

    }
}
