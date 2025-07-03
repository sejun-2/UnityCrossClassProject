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
    [SerializeField] string dieCauseString; // ����������� ���� ��.
    [SerializeField] GameObject guideUIObj;

    private IEnumerator typingTitleCoroutine;
    private IEnumerator typingDieCauseCoroutine;
    private void OnEnable()
    {
        StartTypeEffect();
    }

    // �ܺο��� ȣ���� �� �ִ� Ÿ���� ȿ�� ���� �Լ�
    public void StartTypeEffect()
    {
        //ó���� ������ ���ƾ� �� UI ������Ʈ ��Ȱ��ȭ
        if(guideUIObj.activeSelf)
        {
            guideUIObj.SetActive(false);
        }
        if(dieCauseLabel.gameObject.activeSelf)
        {
            dieCauseLabel.gameObject.SetActive(false);
        }
        // �̹� ���� ���� �ڷ�ƾ�� �ִٸ� ����
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

    // Ÿ���� ����Ʈ �ڷ�ƾ

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
    // ��� ���� �ؽ�Ʈ ���
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
