using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverTextEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleLabel;
    [SerializeField] TextMeshProUGUI dieCauseLabel;
    [SerializeField] float effectDelayTime;

    [SerializeField] string titleString = "YOU DIED";
    [SerializeField] string dieCauseString; // 사망원인으로 사용될 값.
    [SerializeField] GameObject guideUIObj;

    private IEnumerator typingTitleCoroutine;
    private IEnumerator typingDieCauseCoroutine;
    private void OnEnable()
    {
        StartTypeEffect();
    }

    // 외부에서 호출할 수 있는 타이핑 효과 시작 함수
    public void StartTypeEffect()
    {
        //처음엔 보이지 말아야 할 UI 오브젝트 비활성화
        if(guideUIObj.activeSelf)
        {
            guideUIObj.SetActive(false);
        }
        if(dieCauseLabel.gameObject.activeSelf)
        {
            dieCauseLabel.gameObject.SetActive(false);
        }
        // 이미 진행 중인 코루틴이 있다면 중지
        if (typingTitleCoroutine != null)
        {
            StopCoroutine(typingTitleCoroutine);
            typingTitleCoroutine = null;
        }
        if(typingDieCauseCoroutine != null)
        {
            StopCoroutine(typingDieCauseCoroutine);
            typingDieCauseCoroutine = null;
        }
        typingTitleCoroutine = TypeEffectTitleCoroutine(titleString);
        typingDieCauseCoroutine = TypeEffectCauseCoroutine(dieCauseString);
        StartCoroutine(typingTitleCoroutine);
    }

    // 타이핑 이펙트 코루틴

    private IEnumerator TypeEffectTitleCoroutine(string textToShow)
    {
        titleLabel.text = "";

        foreach (char text in textToShow)
        {
            titleLabel.text += text;
            yield return new WaitForSeconds(effectDelayTime);
        }

        typingTitleCoroutine = null;
        StartCoroutine(typingDieCauseCoroutine);
        yield return null;
    }
    // 사망 사유 텍스트 출력
    private IEnumerator TypeEffectCauseCoroutine(string textToCause)
    {
        dieCauseLabel.text = "";
        dieCauseLabel.gameObject.SetActive(true);
        foreach (char text in textToCause)
        {
            dieCauseLabel.text += text;
            yield return new WaitForSeconds(effectDelayTime);
        }

        typingDieCauseCoroutine = null;
        guideUIObj.SetActive(true);
        yield return null;
    }
}
