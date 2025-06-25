using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPopUp : BaseUI
{
    public TextMeshProUGUI uiText;         // 출력할 UI Text
    public string message;      // 출력할 대사
    public float typingSpeed = 0.05f; // 글자 출력 간격(초)

    void Awake()
    {
        uiText = GetUI<TextMeshProUGUI>("StoryText");
        message = "안녕하세요! 이 메시지는 타이핑 효과로 출력됩니다. " +
            "이것은 Unity에서 코루틴을 사용하여 글자를 하나씩 출력하는 예제입니다.";      // 출력할 대사
    }

    void Start()
    {
        Debug.Log(uiText);
        //StartCoroutine(TypeTextPopUp(message));
        uiText.text = message; // 메시지를 바로 출력합니다.
    }

    IEnumerator TypeTextPopUp(string text)
    {
        uiText.text = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append(text[i]);
            uiText.text = stringBuilder.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }
    }


}
