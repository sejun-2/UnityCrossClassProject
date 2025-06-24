using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Inventory Inven;

    // �Ϸ簡 ������ �� ��, �� ���̽�ķ���� ���ƿ� �� �߻�
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
