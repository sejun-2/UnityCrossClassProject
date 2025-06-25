using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPopUp : BaseUI
{
    public TextMeshProUGUI uiText;         // ����� UI Text
    public string message;      // ����� ���
    public float typingSpeed = 0.05f; // ���� ��� ����(��)

    void Awake()
    {
        uiText = GetUI<TextMeshProUGUI>("StoryText");
        message = "�ȳ��ϼ���! �� �޽����� Ÿ���� ȿ���� ��µ˴ϴ�. " +
            "�̰��� Unity���� �ڷ�ƾ�� ����Ͽ� ���ڸ� �ϳ��� ����ϴ� �����Դϴ�.";      // ����� ���
    }

    void Start()
    {
        Debug.Log(uiText);
        //StartCoroutine(TypeTextPopUp(message));
        uiText.text = message; // �޽����� �ٷ� ����մϴ�.
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
