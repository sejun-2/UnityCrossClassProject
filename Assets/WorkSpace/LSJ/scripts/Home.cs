using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : BaseUI
{
    private void Start()
    {
        Debug.Log(GetEvent("Story")); // Home UI가 시작될 때 로그를 출력합니다.
        // 스토리 버튼이 클릭되었을 때 호출되는 메서드
        GetEvent("Story").Click += data => Manager.UI.PopUp.ShowPopUp<TextPopUp>(); // 스토리 팝업을 표시합니다.
    }
    
}
