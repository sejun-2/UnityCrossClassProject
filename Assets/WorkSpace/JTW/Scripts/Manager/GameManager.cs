using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Inventory Inven;

    // 하루가 마무리 될 때, 즉 베이스캠프로 돌아올 때 발생
    public event Action OnDayCompleted;

    public void ChangeScene(string sceneName)
    {
        if(sceneName == "BaseCamp")
        {
            OnDayCompleted?.Invoke();
        }

        SceneManager.LoadScene(sceneName);
    }
}
