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
            Destroy(this.gameObject);

            string dialogueId = Manager.Data.StoryDescriptionData.Values[_storyId].PlayerDialogueID;
            Manager.UI.Inven.ShowBubbleText(Manager.Data.PlayerDialogueData.Values[dialogueId].Dialogue_kr);
        }
    }

    public void InitStoryPopUp(string storyId)
    {
        _storyId = storyId;

        UpdateItemInfo();
    }

    private void UpdateItemInfo()
    {
        StoryDescriptionData data = Manager.Data.StoryDescriptionData.Values[_storyId];

        GetUI<TextMeshProUGUI>("ItemNameText").text = data.Name;
        GetUI<TextMeshProUGUI>("ItemDescriptionText").text = data.Description;
    }
}
