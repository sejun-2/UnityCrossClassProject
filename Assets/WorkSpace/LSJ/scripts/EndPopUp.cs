using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPopUp : BaseUI
{
    void Start()
    {
        Debug.Log("EndPopUp");
        GetEvent("Cancellation").Click += data => Manager.UI.PopUp.ClosePopUp();
        GetEvent("XButton").Click += data => Manager.UI.PopUp.ClosePopUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.UI.PopUp.ClosePopUp();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
