using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsv2 : MonoBehaviour
{
    public float health = 100f;// �÷��̾��� ���� ü�� (0~100)
    public float hunger = 100f;// ����� ��ġ (0�̸� ���ָ� ����, 100�� ����)
    public float fatigue = 0f;// �Ƿε� ��ġ (0�̸� ����, 100�̸� Ż��)
    public float mental = 100f;// ���ŷ� ��ġ (���� �ر� �ý��� ���� ����)
    public float hungerDecayRate = 5f;// ����� ���� �ӵ� (�ʴ� 5�� ����)
    public float fatigueIncreaseRate = 5f;// �Ƿ� ���� �ӵ� (�ʴ� 5�� ����)

    void Update()
    {
        hunger -= hungerDecayRate * Time.deltaTime;// ������� �ð��� �������� ����
        fatigue += fatigueIncreaseRate * Time.deltaTime;// �Ƿε��� �ð��� �������� ����
        // ����İ� �Ƿε��� 0~100 ������ ����
        hunger = Mathf.Clamp(hunger, 0f, 100f);
        fatigue = Mathf.Clamp(fatigue, 0f, 100f);

        // ������� 0�̰ų� �Ƿΰ� 100 �̻��̸� ü�� ���� ����
        if (hunger <= 0 || fatigue >= 100f)
        {
            health -= Time.deltaTime * 5f;// ü���� �ʴ� 5�� ����
        }

        if (health <= 0f)// ü���� 0 ���Ϸ� �������� ��� ó��
        {
            Die();
        }
    }
    void Die()// �÷��̾� ��� �� ȣ��Ǵ� �Լ�
    {
        Debug.Log("�÷��̾� ���");  // �ֿܼ� ��� �޽��� ���
        // ���� ���ӿ��� ���⿡ ���� ���� UI, ������ �� �߰� ����
    }
}
