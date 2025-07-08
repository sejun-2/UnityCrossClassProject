using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapPoint : BaseUI
{
    [SerializeField] private string _sceneName;
    public string SceneName => _sceneName;
    [SerializeField] private string _mapName;
    public string MapName => _mapName;
    [SerializeField] private string _mapDescription;

    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _descriptionText;
    private GameObject _descriptionPanel;
    private GameObject _outlineImage;

    public void InitData()
    {
        _nameText = GetUI<TextMeshProUGUI>("NameText");
        _descriptionText = GetUI<TextMeshProUGUI>("DescriptionText");
        _descriptionPanel = GetUI("DescriptionPanel");
        _outlineImage = GetUI("OutlineImage");

        _nameText.text = _mapName;
        _descriptionText.text = _mapDescription;
    }

    public void SetSelect(bool value)
    {
        _descriptionPanel.SetActive(value);
        _outlineImage.SetActive(value);
    }
}
