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
    public Dictionary<string, bool> IsUsedObject = new Dictionary<string, bool>();
    public Dictionary<string, bool> IsGetSubStory = new Dictionary<string, bool>();

    public bool IsInBaseCamp = true;
    public string SelectedMapName = "";


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
            DayComplete();
        }

        SceneManager.LoadScene(sceneName);
    }

    public void DayComplete()
    {
        foreach(string key in IsUsedObject.Keys)
        {
            IsUsedObject[key] = false;
        }

        IsInBaseCamp = true;

        OnDayCompleted?.Invoke();
    }
}
