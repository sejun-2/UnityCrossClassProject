using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DangerPopUpPresenter : BaseUI
{
    [SerializeField] private AudioClip _closeSound;

    private TextMeshProUGUI _descriptoinText;
    private PlayerStats Stats => Manager.Player.Stats;

    private void Start()
    {
        _descriptoinText = GetUI<TextMeshProUGUI>("DescriptionText");

        _descriptoinText.text = "";

        if (Manager.Game.BarricadeHp <= 30)
        {
            _descriptoinText.text += "바리케이드 : <color=#FF4444>위험</color>\n";
        }
        else if(Manager.Game.BarricadeHp <= 60)
        {
            _descriptoinText.text += "바리케이드 : <color=#FF8C00>주의</color>\n";
        }

        if (Stats.Hunger.Value <= 40)
        {
            _descriptoinText.text += "배고픔 : <color=#FF4444>위험</color>\n";
        }
        else if (Stats.Hunger.Value <= 80)
        {
            _descriptoinText.text += "배고픔 : <color=#FF8C00>주의</color>\n";
        }

        if (Stats.Thirst.Value <= 30)
        {
            _descriptoinText.text += "갈증 : <color=#FF4444>위험</color>\n";
        }
        else if (Stats.Thirst.Value <= 60)
        {
            _descriptoinText.text += "갈증 : <color=#FF8C00>주의</color>\n";
        }

        if (Stats.Mentality.Value <= 15)
        {
            _descriptoinText.text += "정신력 : <color=#FF4444>위험</color>\n";
        }
        else if (Stats.Mentality.Value <= 30)
        {
            _descriptoinText.text += "정신력 : <color=#FF8C00>주의</color>\n";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Sound.SfxPlay(_closeSound, Camera.main.transform);
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(Manager.Game.BarricadeHp <= 30)
            {
                Manager.Game.ChangeScene("GameOverScene");
            }
            else
            {
                Manager.Player.BuffStats.ApplyBuff();
                Manager.Game.IsInBaseCamp = false;
                Manager.Game.ChangeScene(Manager.Game.SelectedSceneName);
            }

        }
    }
}
