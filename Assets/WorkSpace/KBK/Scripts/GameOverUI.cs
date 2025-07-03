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
    { DeathReason.HpZero, "��� ���� : ġ������ ������ ����." },
    { DeathReason.BarricadeBroken, "��� ���� : ������ �ٸ����̵�� ���� ħ��." },
    { DeathReason.Hunger, "��� ���� : �ƻ�." },
    { DeathReason.Thirst, "��� ���� : Ż��." },
    { DeathReason.MentalBreak, "��� ���� : �����Ͽ� ����ұ��� ����." }
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
        Debug.Log("ü��0���� ���");
        ShowOnce(DeathReason.HpZero);
    }
    void BarricadeDestroyed()
    {
        Time.timeScale = 0f;
        Debug.Log("�ٸ�����Ʈ ������0���� ���");
        ShowOnce(DeathReason.BarricadeBroken);
    }
    void DiedByThrist()
    {
        Time.timeScale = 0f;
        Debug.Log("�񸶸�0���� ���");
        ShowOnce(DeathReason.Thirst);
    }
    void DiedByHunger()
    {
        Time.timeScale = 0f;
        Debug.Log("�����0���� ���");
        ShowOnce(DeathReason.Hunger);
    }
    
    void DiedByCrazy()
    {
        Time.timeScale = 0f;
        Debug.Log("���ŷ�0���� ���");
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
