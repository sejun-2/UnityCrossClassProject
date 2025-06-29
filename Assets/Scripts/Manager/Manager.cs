using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager
{
    public static PlayerManager Player => PlayerManager.GetInstance();
    public static UIManager UI => UIManager.GetInstance();
    public static GameManager Game => GameManager.GetInstance();
    public static DataManager Data => DataManager.GetInstance();
    public static SoundManager Sound => SoundManager.GetInstance();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initailize()
    {
        PlayerManager.CreateInstance();
        UIManager.CreateInstance();
        GameManager.CreateInstance();
        DataManager.CreateInstance();
        SoundManager.CreateInstance();
    }
}
