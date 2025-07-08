using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer2 : MonoBehaviour
{
    public TextMeshProUGUI timerText; //TMP_Text -> �ؽ�Ʈ �޽� ����_Text
    private float elapsedTime = 0f; // ��� �ð�
    private bool isRunning = true;

    private static Timer instance;

    void Awake()
    {
        // �̱��� �������� �ߺ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // ����� �ð� �ҷ�����
            if (PlayerPrefs.HasKey("ElapsedTime"))
                elapsedTime = PlayerPrefs.GetFloat("ElapsedTime");
            else
                elapsedTime = 0f;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        int totalSeconds = Mathf.FloorToInt(elapsedTime);

        int days = totalSeconds / 86400;
        int hours = (totalSeconds % 86400) / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        string map;
        if (Manager.Game.IsInBaseCamp)
            map = "���̽� ķ��";
        else
            map = Manager.Game.SelectedMapName;

        timerText.text = string.Format(" ������ : {0}�� {1:00} �ð� {2:00} �� {3:00} �� \n {4}", days, hours, minutes, seconds, map);
    }

    public void StopTimer() // Ÿ�̸Ӹ� ����
    {
        isRunning = false;
    }

    public void AddDay()    // �Ϸ縦 �߰�
    {
        elapsedTime += 86400f;
        UpdateTimerText();
    }

    public void AddSeconds(float seconds)   // Ư�� ��(�ð�)�� �߰�
    {
        elapsedTime += seconds;
        UpdateTimerText();
    }

    // �� ��ȯ, ���̺� ��� ȣ���� ����
    public void SaveElapsedTime()
    {
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.Save();
    }

    // �ʿ��ϴٸ� �ܺο��� Ÿ�̸Ӹ� ������ �� �ֵ���
    public void ResetTimer()        // ���� ����� �Ǵ� Ư�� �̺�Ʈ���� ȣ��
    {
        elapsedTime = 0f;
        UpdateTimerText();
        SaveElapsedTime();
    }

    // ����: ���� ���� �� �ڵ� ����
    void OnApplicationQuit()
    {
        SaveElapsedTime();
    }

}
