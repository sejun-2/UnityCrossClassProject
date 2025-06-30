using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private void Awake()
    {
        Manager.Player.Transform = transform;
    }
}
