using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : BaseUI
{
    private void Start()
    {
        // ���丮 ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
        GetEvent("Story").Click += data => Manager.UI.PopUp.ShowPopUp<TextPopUp>(); // ���丮 �˾��� ǥ���մϴ�.
    }
    
}
