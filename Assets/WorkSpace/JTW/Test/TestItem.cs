using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestItem", menuName = "Item/TestItem")]
public class TestItem : Item
{
    public override void Use()
    {
        Debug.Log("아이템 사용");
    }
}
