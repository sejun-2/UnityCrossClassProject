using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryInteractionPopUpPresenter : BaseUI
{
    [SerializeField] private AudioClip _closeSound;

    private Image _storyImage;
    private TextMeshProUGUI _descriptionText;

    private string _storyInteractionId;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            List<string> dialogueList = Manager.Data.StoryInteractionData.Values[_storyInteractionId].PlayerDialogueIndexList;

            Manager.UI.Inven.ShowBubbleText(dialogueList);

            Manager.Player.Stats.isFarming = false;
            Destroy(gameObject);
        }
    }

    public void InitStory(string storyInteractionId)
    {
        _storyInteractionId = storyInteractionId;

        _storyImage = GetUI<Image>("StoryImage");
        _descriptionText = GetUI<TextMeshProUGUI>("DescriptionText");

        StoryInteractionData data = Manager.Data.StoryInteractionData.Values[storyInteractionId];

        _storyImage.sprite = data.Icon;
        _descriptionText.text = data.Description_kr;
    }
}
