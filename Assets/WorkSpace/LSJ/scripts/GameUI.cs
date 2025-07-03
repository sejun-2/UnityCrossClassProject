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

        // ó�� UI�� ������ ��, �ѹ��� ������ ���� �°� value ���� ����
        HungerSlider.value = Manager.Player.Stats.Hunger.Value;
        ThirstSlider.value = Manager.Player.Stats.Thirst.Value;
        MentalitySlider.value = Manager.Player.Stats.Mentality.Value;

        // Player의 Buff 값이 바뀔 때마다 UI 갱신
        Manager.Player.Stats.Buff.OnChanged += UpdateBuffIcon;
        UpdateBuffIcon(Manager.Player.Stats.Buff.Value); // 최초 상태도 반영
    }

    private void Update()
    {

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
        // �� ���� ���� ��Ȳ���� Image ��ü�� Destroy �Ǵ� ��Ȳ�� �߻�
        // �׶� Image ������ Null���� ���°� ������ null üũ
        if (BuffsImage == null) return;

        // Nomal(0)�� ���� �̹��� ����
        if (buff == PlayerBuffs.Nomal)
        {
            //if (BuffsImage != null)
                BuffsImage.enabled = false; // 이미지 숨김
            return;
        }

        // PlayerBuffs를 int로 변환하여 인덱스로 사용 -> PlayerBuffs 열거형의 값은 0부터 시작하므로 직접 인덱스로 사용 가능
        int idx = (int)buff;
        // BuffIcons 배열이 null이 아니고, 인덱스가 유효한지 확인
        if (BuffsImage != null && BuffIcons != null && idx >= 0 && idx < BuffIcons.Length)
        {
            // 아이콘이 존재하는 경우에만 이미지 업데이트
            BuffsImage.sprite = BuffIcons[idx];
            // 아이콘이 존재하면 이미지 표시
            BuffsImage.enabled = true;
        }
        else
        {
            BuffsImage.enabled = false; // 혹시 아이콘이 없으면 이미지 숨김
        }
    }

}
