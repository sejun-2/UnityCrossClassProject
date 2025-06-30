using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShowBubbleText : MonoBehaviour
{
    [SerializeField] private string _text;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Manager.UI.Inven.ShowBubbleText(_text);
        }
    }
}
