using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string content)
    {
        _text.text = content;
        RectTransform rt = _text.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Mathf.Min(600, _text.preferredWidth), 30);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, _text.preferredHeight);
    }
}
