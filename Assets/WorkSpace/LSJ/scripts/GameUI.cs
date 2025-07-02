using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Slider HpSlider; // Inspector에서 HP 슬라이더 연결
    [SerializeField] private Slider HungerSlider; 
    [SerializeField] private Slider ThirstSlider; 
    [SerializeField] private Slider MentalitySlider;

    [SerializeField] public Image BuffsImage; // 버프 이미지 연결
    public Sprite[] BuffIcons; // PlayerBuffs 순서대로 Sprite 할당

    private void Start()
    {
        //초기화 시 hpSlider와 playerStats가 할당되었는지 확인
        if (HpSlider == null)
            Debug.LogError("hpSlider가 할당되지 않았습니다!");
        if (Manager.Player.Stats == null)
        {
            Debug.LogError("playerStats가 할당되지 않았습니다!");
        }
        //Manager.Player.Stats.MaxHp.Value = 100; // 기본값 설정
        //Manager.Player.Stats.CurHp.Value = 60; // 기본값 설정 

        // 체력 변화 이벤트 구독
        Manager.Player.Stats.CurHp.OnChanged += OnCurHpChanged;
        Manager.Player.Stats.MaxHp.OnChanged += OnMaxHpChanged;

        UpdateHpBar(Manager.Player.Stats.CurHp.Value, Manager.Player.Stats.MaxHp.Value);

        Manager.Player.Stats.Hunger.OnChanged += (newHunger) => HungerSlider.value = newHunger;
        Manager.Player.Stats.Thirst.OnChanged += (newWater) => ThirstSlider.value = newWater;
        Manager.Player.Stats.Mentality.OnChanged += (newMentality) => MentalitySlider.value = newMentality;

        // 초기 버프 상태를 Nomal로 설정
        Manager.Player.Stats.Buff.Value = PlayerBuffs.Nomal;

        // Player의 Buff 값이 바뀔 때마다 UI 갱신
        Manager.Player.Stats.Buff.OnChanged += UpdateBuffIcon;
        UpdateBuffIcon(Manager.Player.Stats.Buff.Value); // 최초 상태도 반영
    }

    private void Update()
    {


        // 버프 이미지 업데이트

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
        // Nomal(0)일 때는 이미지 숨김
        if (buff == PlayerBuffs.Nomal)
        {
            //if (BuffsImage != null)
                BuffsImage.enabled = false;
            return;
        }

        // PlayerBuffs를 int로 변환하여 인덱스로 사용
        int idx = (int)buff;
        if (BuffsImage != null && BuffIcons != null && idx >= 0 && idx < BuffIcons.Length)
        {
            BuffsImage.sprite = BuffIcons[idx];
            BuffsImage.enabled = true;
        }
        else
        {
            BuffsImage.enabled = false; // 혹시 아이콘이 없으면 이미지 숨김
        }
    }

}
