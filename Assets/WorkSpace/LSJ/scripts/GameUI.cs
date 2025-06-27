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
    [SerializeField] public int setMaxHp = 100; // 최대 체력
    [SerializeField] public int setCurHp = 80; // 최대 체력

    private void Start()
    {
        //초기화 시 hpSlider와 playerStats가 할당되었는지 확인
        if (HpSlider == null)
            Debug.LogError("hpSlider가 할당되지 않았습니다!");
        if (Manager.Player.Stats == null)
        {
            Debug.LogError("playerStats가 할당되지 않았습니다!");
        }

    }

    private void Update()
    {
        Manager.Player.Stats.MaxHp.Value = setMaxHp; // 기본값 설정
        Manager.Player.Stats.CurHp.Value = setCurHp; // 기본값 설정 

        // 체력 변화 이벤트 구독
        Manager.Player.Stats.CurHp.OnChanged += OnCurHpChanged;
        Manager.Player.Stats.MaxHp.OnChanged += OnMaxHpChanged;
        UpdateHpBar(Manager.Player.Stats.CurHp.Value, Manager.Player.Stats.MaxHp.Value);
        Debug.LogError("MaxHp값 : " + Manager.Player.Stats.MaxHp.Value);
        Debug.LogError("CurHp값 : " + Manager.Player.Stats.CurHp.Value);

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
