using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeTest : MonoBehaviour
{
    [SerializeField] InventoryCanvas invenCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            invenCanvas.ShowTradeItemBox();
        }
    }
}
