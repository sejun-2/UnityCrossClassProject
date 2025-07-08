using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer2 : MonoBehaviour
{
    public TextMeshProUGUI timerText; //TMP_Text -> 텍스트 메시 프로_Text
    private float elapsedTime = 0f; // 경과 시간
    private bool isRunning = true;

    private static Timer instance;

    void Awake()
    {
        // 싱글톤 패턴으로 중복 방지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // 저장된 시간 불러오기
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
            map = "베이스 캠프";
        else
            map = Manager.Game.SelectedMapName;

        timerText.text = string.Format(" 생존일 : {0}일 {1:00} 시간 {2:00} 분 {3:00} 초 \n {4}", days, hours, minutes, seconds, map);
    }

    public void StopTimer() // 타이머를 중지
    {
        isRunning = false;
    }

    public void AddDay()    // 하루를 추가
    {
        elapsedTime += 86400f;
        UpdateTimerText();
    }

    public void AddSeconds(float seconds)   // 특정 초(시간)를 추가
    {
        elapsedTime += seconds;
        UpdateTimerText();
    }

    // 씬 전환, 세이브 등에서 호출해 저장
    public void SaveElapsedTime()
    {
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.Save();
    }

    // 필요하다면 외부에서 타이머를 리셋할 수 있도록
    public void ResetTimer()        // 게임 종료시 또는 특정 이벤트에서 호출
    {
        elapsedTime = 0f;
        UpdateTimerText();
        SaveElapsedTime();
    }

    // 예시: 게임 종료 시 자동 저장
    void OnApplicationQuit()
    {
        SaveElapsedTime();
    }

}
