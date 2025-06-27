using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Inventory Inven;
    public ItemBoxData ItemBox;

    public Dictionary<string, bool> IsRepairObject = new Dictionary<string, bool>();


    // �Ϸ簡 ������ �� ��, �� ���̽�ķ���� ���ƿ� �� �߻�
    public event Action OnDayCompleted;

    private void Awake()
    {
        Inven = new Inventory();
        ItemBox = new ItemBoxData();
    }

    public void ChangeScene(string sceneName)
    {
        if(sceneName == "BaseCamp")
        {
            OnDayCompleted?.Invoke();
        }

        SceneManager.LoadScene(sceneName);
    }
}
