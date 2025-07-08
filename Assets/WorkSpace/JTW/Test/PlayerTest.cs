using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private void Awake()
    {
        Manager.Player.Transform = transform;
    }

    private void Update()
    {
        if(!Manager.Game.IsInBaseCamp && Input.GetKeyDown(KeyCode.A))
        {
            Manager.UI.Inven.ShowInven();
        }
    }
}
