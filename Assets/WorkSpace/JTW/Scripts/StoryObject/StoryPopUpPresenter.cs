using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryPopUpPresenter : BaseUI
{
    private string _storyId;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Player.Stats.isFarming = false;
            Destroy(this.gameObject);
        }
    }

    public void InitStoryPopUp(string storyId)
    {
        _storyId = storyId;

        UpdateItemInfo();
    }

    private void UpdateItemInfo()
    {
        CollectionData data = Manager.Data.CollectionData.Values[_storyId];

        GetUI<TextMeshProUGUI>("ItemNameText").text = data.Name;
        GetUI<TextMeshProUGUI>("ItemDescriptionText").text = data.Description;
    }
}
