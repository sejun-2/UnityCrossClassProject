using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEndObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Image _fadeImage;

    public void Interact()
    {
        if (Manager.Game.IsTalkDialogue["20010"] && Manager.Game.IsTalkDialogue["20009"])
        {
            Manager.UI.Inven.ShowBubbleText("20014");
            Manager.Player.Stats.isFarming = true;
            StartCoroutine(EndTutorialCoroutine());
        }
        else
        {
            Manager.UI.Inven.ShowBubbleText("20013");
        }
    }

    private IEnumerator EndTutorialCoroutine()
    {
        yield return new WaitForSeconds(3f);

        _fadeImage.DOFade(1f, 2f);


        yield return new WaitForSeconds(4f);

        Manager.Game.ChangeScene("BaseCamp");
    }
}
