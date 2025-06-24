using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private PopUpCanvas _popUpCanvas = new();   // PopUpCanvas의 인스턴스를 관리하기 위한 필드
    public PopUpCanvas PopUp => _popUpCanvas.Instance;  // PopUpCanvas의 인스턴스를 반환하는 프로퍼티
}
