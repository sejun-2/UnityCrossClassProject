using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryPresenter : BaseUI
{
    [SerializeField] private ItemSlotUIs _diarySlotsPrefab;

    private TextMeshProUGUI _descriptionText;
    private GameObject _slotPanel;

    private ItemSlotUIs _diarySlotUIs;

    private List<string> _diaryId = new List<string>();

    private void Start()
    {
        InitSubStoryUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DiaryData data = Manager.Data.DiaryData.Values[_diaryId[_diarySlotUIs.SelectedSlotIndex]];

            Manager.Player.Stats.isFarming = false;

            Manager.UI.Inven.ShowBubbleText(data.PlayerDialogueIndexList);
            Destroy(this.gameObject);

        }

        MoveInventory();
    }

    public void InitSubStoryUI()
    {
        _descriptionText = GetUI<TextMeshProUGUI>("DescriptionText");
        _slotPanel = GetUI("SlotPanel");

        _diarySlotUIs = Instantiate(_diarySlotsPrefab, _slotPanel.transform).GetComponent<ItemSlotUIs>();
        _diarySlotUIs.SetLineCount(1);

        foreach (string key in Manager.Game.IsGetDiary.Keys.ToArray())
        {
            int index = 0;
            if (Manager.Game.IsGetDiary[key])
            {
                _diarySlotUIs.AddSlotUI();
                _diaryId.Add(key);
                _diarySlotUIs.SlotUIs[index].SetText(Manager.Data.DiaryData.Values[key].Name);
                index++;
            }
        }

        _diarySlotUIs.SetPanelSize(new Vector2(1, 4));

        if (_diarySlotUIs.SlotUIs.Count != 0)
        {
            _diarySlotUIs.SelectSlotUI(0);
            UpdateItemInfo();
        }
        else
        {
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = "";
        }
    }

    private void MoveInventory()
    {
        if (_diarySlotUIs.SlotUIs.Count <= 0) return;

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

    private void ChangeSelectSlot(Vector2 movePos)
    {
        _diarySlotUIs.MoveSelectSlot(movePos);
        UpdateItemInfo();

        RectTransform rt = _diarySlotUIs.SlotUIs[_diarySlotUIs.SelectedSlotIndex].GetComponent<RectTransform>();
    }

    private void UpdateItemInfo()
    {
        DiaryData data = Manager.Data.DiaryData.Values[_diaryId[_diarySlotUIs.SelectedSlotIndex]];

        if (data != null)
        {
            _descriptionText.text = data.Description;
        }
        else
        {
            _descriptionText.text = "";
        }
    }
}
