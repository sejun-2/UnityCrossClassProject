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

    [SerializeField] public Image BuffsImage; // ���� �̹��� ����
    public Sprite[] BuffIcons; // PlayerBuffs ������� Sprite �Ҵ�

    private void Start()
    {
        //�ʱ�ȭ �� hpSlider�� playerStats�� �Ҵ�Ǿ����� Ȯ��
        if (HpSlider == null)
            Debug.LogError("hpSlider�� �Ҵ���� �ʾҽ��ϴ�!");
        if (Manager.Player.Stats == null)
        {
            Debug.LogError("playerStats�� �Ҵ���� �ʾҽ��ϴ�!");
        }
        //Manager.Player.Stats.MaxHp.Value = 100; // �⺻�� ����
        //Manager.Player.Stats.CurHp.Value = 60; // �⺻�� ���� 

        // ü�� ��ȭ �̺�Ʈ ����
        Manager.Player.Stats.CurHp.OnChanged += OnCurHpChanged;
        Manager.Player.Stats.MaxHp.OnChanged += OnMaxHpChanged;

        UpdateHpBar(Manager.Player.Stats.CurHp.Value, Manager.Player.Stats.MaxHp.Value);

        Manager.Player.Stats.Hunger.OnChanged += (newHunger) => HungerSlider.value = newHunger;
        Manager.Player.Stats.Thirst.OnChanged += (newWater) => ThirstSlider.value = newWater;
        Manager.Player.Stats.Mentality.OnChanged += (newMentality) => MentalitySlider.value = newMentality;

        // �ʱ� ���� ���¸� Nomal�� ����
        Manager.Player.Stats.Buff.Value = PlayerBuffs.Nomal;

        // Player�� Buff ���� �ٲ� ������ UI ����
        Manager.Player.Stats.Buff.OnChanged += UpdateBuffIcon;
        UpdateBuffIcon(Manager.Player.Stats.Buff.Value); // ���� ���µ� �ݿ�
    }

    private void Update()
    {


        // ���� �̹��� ������Ʈ

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

    public void UpdateBuffIcon(PlayerBuffs buff)
    {
        // Nomal(0)�� ���� �̹��� ����
        if (buff == PlayerBuffs.Nomal)
        {
            //if (BuffsImage != null)
                BuffsImage.enabled = false;
            return;
        }

        // PlayerBuffs�� int�� ��ȯ�Ͽ� �ε����� ���
        int idx = (int)buff;
        if (BuffsImage != null && BuffIcons != null && idx >= 0 && idx < BuffIcons.Length)
        {
            BuffsImage.sprite = BuffIcons[idx];
            BuffsImage.enabled = true;
        }
        else
        {
            BuffsImage.enabled = false; // Ȥ�� �������� ������ �̹��� ����
        }
    }

}
