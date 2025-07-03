using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private Transform popupRoot;
    public GameObject gameOverPrefab;
    bool isGameOver = false;

    enum DeathReason
    {
        HpZero, BarricadeBroken, Hunger, Thirst, MentalBreak
    }

    Dictionary<DeathReason, string> deathMessages = new()
{
    { DeathReason.HpZero, "사망 사유 : 치명적인 공격을 받음." },
    { DeathReason.BarricadeBroken, "사망 사유 : 무너진 바리케이드로 좀비가 침입." },
    { DeathReason.Hunger, "사망 사유 : 아사." },
    { DeathReason.Thirst, "사망 사유 : 탈수." },
    { DeathReason.MentalBreak, "사망 사유 : 절망하여 좀비소굴에 돌진." }
};
    private void Awake()
    {
        popupRoot = FindObjectOfType<Canvas>().transform;
    }
    private void Update()
    {
        GameOver();
    }

    void GameOver()
    {
        if (isGameOver)
            return;

        if (Manager.Player.Stats.CurHp.Value <= 0)
        {
            DiedByHp0();
        }
        else if (Manager.Game.BarricadeHp <= 0)
        {
            BarricadeDestroyed();
        }
        else if (Manager.Player.Stats.Thirst.Value <= 0)
        {
            DiedByThrist();
        }
        else if (Manager.Player.Stats.Hunger.Value <= 0)
        {
            DiedByHunger();
        }
        else if (Manager.Player.Stats.Mentality.Value <= 0)
        {
            DiedByCrazy();
        }
    }

    void DiedByHp0()
    {
        Time.timeScale = 0f;
        Debug.Log("체력0으로 사망");
        ShowOnce(DeathReason.HpZero);
    }
    void BarricadeDestroyed()
    {
        Time.timeScale = 0f;
        Debug.Log("바리게이트 내구도0으로 사망");
        ShowOnce(DeathReason.BarricadeBroken);
    }
    void DiedByThrist()
    {
        Time.timeScale = 0f;
        Debug.Log("목마름0으로 사망");
        ShowOnce(DeathReason.Thirst);
    }
    void DiedByHunger()
    {
        Time.timeScale = 0f;
        Debug.Log("배고픔0으로 사망");
        ShowOnce(DeathReason.Hunger);
    }
    
    void DiedByCrazy()
    {
        Time.timeScale = 0f;
        Debug.Log("정신력0으로 사망");
        ShowOnce(DeathReason.MentalBreak);
    }
    void ShowOnce(DeathReason reason)
    {
        isGameOver = true;
        OpenGameOver(deathMessages[reason]);
    }
    public void OpenGameOver(string message)
    {
        GameObject popup = Instantiate(gameOverPrefab, popupRoot);

        RectTransform rt = popup.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        popup.transform.SetAsLastSibling();

        popup.GetComponent<GameOverPopup>().SetMessage(message);
    }
}
