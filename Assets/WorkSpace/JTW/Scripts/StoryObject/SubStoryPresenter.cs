using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubStoryPresenter : BaseUI
{
    [SerializeField] private GameObject _itemSlotsPrefab;
    [SerializeField] private GameObject _slotUIPrefab;

    private Image _itemIconImage;
    private ItemSlotUIs _itemSlotUIs;
    private List<string> _subStroyId = new List<string>();

    private void Start()
    {
        InitSubStoryUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Player.Stats.isFarming = false;
            Destroy(this.gameObject);
        }

        MoveInventory();
    }

    public void InitSubStoryUI()
    {
        _itemIconImage = GetUI<Image>("ItemIconImage");
        Transform itemSlotsPanel = GetUI("Content").transform;

        _itemSlotUIs = Instantiate(_itemSlotsPrefab, itemSlotsPanel).GetComponent<ItemSlotUIs>();
        _itemSlotUIs.SetLineCount(1);

        foreach(string key in Manager.Game.IsGetSubStory.Keys)
        {
            int index = 0;
            if (Manager.Game.IsGetSubStory[key])
            {
                _itemSlotUIs.AddSlotUI();
                _subStroyId.Add(key);
                _itemSlotUIs.SlotUIs[index].SetText(Manager.Data.StoryDescriptionData.Values[key].Name);
                index++;
            }
        }

        _itemSlotUIs.SetPanelSize(new Vector2(1, _itemSlotUIs.SlotUIs.Count));

        if (_itemSlotUIs.SlotUIs.Count != 0)
        {
            _itemSlotUIs.SelectSlotUI(0);
            UpdateItemInfo();
        }
        else
        {
            _itemIconImage.gameObject.SetActive(false);
            GetUI<TextMeshProUGUI>("ItemNameText").text = "";
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = "";
        }
    }

    private void MoveInventory()
    {
        if (_itemSlotUIs.SlotUIs.Count <= 0) return;

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
        _itemSlotUIs.MoveSelectSlot(movePos);
        UpdateItemInfo();

        RectTransform rt = _itemSlotUIs.SlotUIs[_itemSlotUIs.SelectedSlotIndex].GetComponent<RectTransform>();
        ScrollToView(rt);
    }

    private void UpdateItemInfo()
    {
        StoryDescriptionData data = Manager.Data.StoryDescriptionData.Values[_subStroyId[_itemSlotUIs.SelectedSlotIndex]];

        if (data != null)
        {
            _itemIconImage.gameObject.SetActive(true);
            _itemIconImage.sprite = data.Icon;
            GetUI<TextMeshProUGUI>("ItemNameText").text = data.Name;
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = data.Description;
        }
        else
        {
            _itemIconImage.gameObject.SetActive(false);
            GetUI<TextMeshProUGUI>("ItemNameText").text = "";
            GetUI<TextMeshProUGUI>("ItemDescriptionText").text = "";
        }
    }

    void ScrollToView(RectTransform target)
    {
        ScrollRect scrollRect = GetUI<ScrollRect>("ItemSlotsPanel");
        RectTransform content = GetUI<RectTransform>("Content");
        RectTransform viewport = GetUI<RectTransform>("Viewport");

        // Content 기준에서의 위치 계산
        Vector2 localPos = content.InverseTransformPoint(target.position);

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        float size = contentHeight - viewportHeight;
        float normalizedPos = scrollRect.verticalNormalizedPosition;

        if (viewportHeight + (1 - normalizedPos) * size < -(localPos.y - 130))
        {
            scrollRect.verticalNormalizedPosition = 1 - ((-(localPos.y - 130) - viewportHeight) / size);
        }

        if ((1 - normalizedPos) * size > -(localPos.y + 30))
        {
            scrollRect.verticalNormalizedPosition = 1 + (localPos.y + 30) / size;
        }
    }
}
