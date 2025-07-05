using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; //TMP_Text -> �ؽ�Ʈ �޽� ����_Text
    private float elapsedTime = 0f; // ��� �ð�
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
                map = "���̽� ķ��";
            }
            else
            {
                map = Manager.Game.SelectedMapName;
            }
            
            timerText.text = string.Format(" ������ : {0}�� {1:00} �ð� {2:00} �� {3:00} ��,  {4}", days, hours, minutes, seconds, map);
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

}
