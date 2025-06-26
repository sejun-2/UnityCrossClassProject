using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangguagePopUp : BaseUI
{
    void Start()
    {
        Debug.Log("LangguagePopUp");
        GetEvent("Cancellation").Click += data => Manager.UI.PopUp.ClosePopUp();
        GetEvent("XButton").Click += data => Manager.UI.PopUp.ClosePopUp();
    }
}
