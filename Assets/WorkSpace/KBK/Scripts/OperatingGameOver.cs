using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatingGameOver : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("J눌림");
            Manager.Player.Stats.CurHp.Value = 0;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("K눌림");
            Manager.Game.BarricadeHp = 0;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("L눌림");
            Manager.Player.Stats.Thirst.Value = 0;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M눌림");
            Manager.Player.Stats.Hunger.Value = 0;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("N눌림");
            Manager.Player.Stats.Mentality.Value = 0;
        }
    }

}
