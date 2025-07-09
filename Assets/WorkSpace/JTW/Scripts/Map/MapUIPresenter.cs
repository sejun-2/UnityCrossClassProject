using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapUIPresenter : BaseUI
{
    [SerializeField] private AudioClip _closeSound;

    [SerializeField] private GameObject _itemSlotUIsPrefab;

    private GameObject _mapSlotsPanel;
    private ItemSlotUIs _mapSlotUIs;
    private List<MapPoint> _mapPoints;

    private int _selectedMapIndex;

    private void Start()
    {
        InitMapUI();
    }

    private void Update()
    {
        MoveInventory();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.Game.SelectedMapName = _mapPoints[_selectedMapIndex].MapName;
            Manager.Game.SelectedSceneName = _mapPoints[_selectedMapIndex].SceneName;
            Manager.UI.Inven.ShowTradeItemBox();
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Sound.SfxPlay(_closeSound, Camera.main.transform);
            Manager.Player.Stats.isFarming = false;
            Destroy(gameObject);
        }
    }

    private void InitMapUI()
    {
        _mapSlotsPanel = GetUI("MapSlotsPanel");

        _mapSlotUIs = Instantiate(_itemSlotUIsPrefab, _mapSlotsPanel.transform).GetComponent<ItemSlotUIs>();
        _mapSlotUIs.SetPanelSize(new Vector2(1, 5));
        _mapSlotUIs.SetLineCount(1);

        _mapPoints = GetComponentsInChildren<MapPoint>().ToList();

        for(int i = 0; i < _mapPoints.Count; i++)
        {
            _mapPoints[i].InitData();
            _mapSlotUIs.AddSlotUI();
            _mapSlotUIs.SlotUIs[i].SetText(_mapPoints[i].MapName);
        }

        _mapSlotUIs.SelectSlotUI(0);
        UpdateMapDescription();
    }

    private void MoveInventory()
    {
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
        _mapSlotUIs.MoveSelectSlot(movePos);
        UpdateMapDescription();
    }

    private void UpdateMapDescription()
    {
        _mapPoints[_selectedMapIndex].SetSelect(false);

        _selectedMapIndex = _mapSlotUIs.SelectedSlotIndex;

        _mapPoints[_selectedMapIndex].SetSelect(true);
    }
}
