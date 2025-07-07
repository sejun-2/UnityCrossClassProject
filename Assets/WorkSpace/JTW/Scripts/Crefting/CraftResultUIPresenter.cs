using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftResultUIPresenter : BaseUI
{
    [SerializeField] private AudioClip _closeSound;

    private Image _itemIcon;
    private TextMeshProUGUI _nameText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Sound.SfxPlay(_closeSound, Camera.main.transform);
            Destroy(gameObject);
        }
    }

    public void InitData(Item item)
    {
        _itemIcon = GetUI<Image>("ItemIcon");
        _nameText = GetUI<TextMeshProUGUI>("NameText");

        _itemIcon.sprite = item.icon;
        _nameText.text = item.itemName;
    }
}
