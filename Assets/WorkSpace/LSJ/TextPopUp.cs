using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class TextPopUp : BaseUI
{
    public TextMeshProUGUI uiText;         // 출력할 UI Text
    //public string message;      // 출력할 대사
    public string[] messages = {
        "낮에는 포격과 저격수들의 위험으로 때문에 돌아 다닐수 없고",
        "밤에는 강도들이 피난처를 노리는 절망적인 상황입니다.",
        "우리는 살아남기 위해 최선을 다해야 합니다.",
    };

    public float typingSpeed = 0.05f; // 글자 출력 간격(초)
    public float lineDelay = 1.0f; // 한 줄 끝나고 다음 줄까지 대기 시간

    //void Awake()
    //{

    //    message = "안녕하세요! 이 메시지는 타이핑 효과로 출력됩니다. " +
    //        "이것은 Unity에서 코루틴을 사용하여 글자를 하나씩 출력하는 예제입니다."; // 메시지를 바로 출력합니다.

    //}

    void Start()
    {
        Debug.Log(uiText);
        //StartCoroutine(TypeTextPopUp(message));
        StartCoroutine(ShowLinesSequentially());
    }

    IEnumerator TypeTextPopUp(string text)  // 코루틴을 사용하여 글자를 하나씩 출력하는 메서드
    {
        uiText.text = string.Empty; // 초기화
        StringBuilder stringBuilder = new StringBuilder(); // StringBuilder를 사용하여 문자열을 효율적으로 조합
        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append(text[i]); // 현재 글자를 추가
            uiText.text = stringBuilder.ToString(); // UI Text에 적용
            yield return new WaitForSeconds(typingSpeed); // 지정된 시간만큼 대기
        }
    }

    IEnumerator ShowLinesSequentially()
    {
        uiText.text = string.Empty;

        foreach (string line in messages)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(lineDelay);
            uiText.text += "\n";
        }
    }

    IEnumerator TypeLine(string text)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            uiText.text = sb.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }
    }


}
