using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSubStoryUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Manager.UI.Inven.ShowSubStoryUI();
            Manager.Player.Stats.isFarming = true;
        }
    }
}
