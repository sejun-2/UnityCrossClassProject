using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCookingTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Manager.UI.Inven.ShowCookingUI();
        }
    }
}
