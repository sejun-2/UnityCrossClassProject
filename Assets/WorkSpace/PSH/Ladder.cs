using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;

    public Transform GetTop() => topPoint;
    public Transform GetBottom() => bottomPoint;
    public float ExitRange => .6f;

}
