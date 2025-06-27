using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Slider HpSlider; // Inspector���� HP �����̴� ����
    [SerializeField] private Slider HungerSlider; 
    [SerializeField] private Slider ThirstSlider; 
    [SerializeField] private Slider MentalitySlider; 

    private void Start()
    {
        //�ʱ�ȭ �� hpSlider�� playerStats�� �Ҵ�Ǿ����� Ȯ��
        if (HpSlider == null)
            Debug.LogError("hpSlider�� �Ҵ���� �ʾҽ��ϴ�!");
        if (Manager.Player.Stats == null)
        {
            Debug.LogError("playerStats�� �Ҵ���� �ʾҽ��ϴ�!");
            Manager.Player.Stats.MaxHp.Value = 100; // �⺻�� ����
            Manager.Player.Stats.CurHp.Value = 40; // �⺻�� ���� 
        }
        Manager.Player.Stats.MaxHp.Value = 100; // �⺻�� ����
        Manager.Player.Stats.CurHp.Value = 60; // �⺻�� ���� 
    }

    private void Update()
    {
        // ü�� ��ȭ �̺�Ʈ ����
        Manager.Player.Stats.CurHp.OnChanged += OnCurHpChanged;
        Manager.Player.Stats.MaxHp.OnChanged += OnMaxHpChanged;
        UpdateHpBar(Manager.Player.Stats.CurHp.Value, Manager.Player.Stats.MaxHp.Value);
        Debug.LogError("MaxHp�� : " + Manager.Player.Stats.MaxHp.Value);
        Debug.LogError("CurHp�� : " + Manager.Player.Stats.CurHp.Value);

        Manager.Player.Stats.Hunger.OnChanged += (newHunger) => HungerSlider.value = newHunger;
        Manager.Player.Stats.Thirst.OnChanged += (newWater) => ThirstSlider.value = newWater;
        //Manager.Player.Stats.Mentality.OnChanged += (newMentality) => MentalitySlider.value = newMentality;
    }

    private void OnCurHpChanged(int newCurHp)
    {
        UpdateHpBar(newCurHp, Manager.Player.Stats.MaxHp.Value);
    }

    private void OnMaxHpChanged(int newMaxHp)
    {
        UpdateHpBar(Manager.Player.Stats.CurHp.Value, newMaxHp);
    }

    private void UpdateHpBar(int curHp, int maxHp)
    {
        if (HpSlider != null)
        {
            HpSlider.maxValue = maxHp;
            HpSlider.value = curHp;
        }
    }

}
