using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFarmingTest : MonoBehaviour
{
    [SerializeField] private FarmingObject _farming;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _farming.Interact();
        }    
    }
}
