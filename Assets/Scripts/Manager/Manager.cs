using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager
{
    public static PlayerManager Player => PlayerManager.GetInstance();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initailize()
    {
        PlayerManager.CreateInstance();
    }
}
