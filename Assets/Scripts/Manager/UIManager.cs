using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private PopUpCanvas _popUpCanvas = new();
    public PopUpCanvas PopUp => _popUpCanvas.Instance;
}
