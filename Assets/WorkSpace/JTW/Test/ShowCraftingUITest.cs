using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCraftingUITest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Manager.UI.Inven.ShowCraftingUI();
        }
    }
}
