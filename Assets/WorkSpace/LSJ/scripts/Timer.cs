using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; //TMP_Text -> 텍스트 메시 프로_Text
    private float elapsedTime = 0f; // 경과 시간
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            int totalSeconds = Mathf.FloorToInt(elapsedTime);

            int days = totalSeconds / 86400;
            int hours = (totalSeconds % 86400) / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;
            string map;
            if (Manager.Game.IsInBaseCamp)
            {
                map = "베이스 캠프";
            }
            else
            {
                map = Manager.Game.SelectedMapName;
            }
            
            timerText.text = string.Format(" 생존일 : {0}일 {1:00} 시간 {2:00} 분 {3:00} 초,  {4}", days, hours, minutes, seconds, map);
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

}
