using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ���� ���¸� �����ϴ� ��ũ��Ʈ (���� ���� �ð� ���� �� ��Ÿ�� ����)
// �� ��ũ��Ʈ�� Renderer ������Ʈ�� �ִ� ������Ʈ���� ���� ����
[RequireComponent(typeof(Renderer))]
public class PlayerStealth : MonoBehaviour
{
    //���� ���� ������
    private bool isHidden = false;        // ���� ���� ������ ����
    private bool canStealth = true;       // ���� ��� �������� ���� 
    private float stealthTimer = 0f;      // ���� �ð� ������ Ÿ�̸�
    //���� ������
    public float maxStealthTime = 5f;     // ���� ������ �ִ� �ð� �ʴ�����
    public float stealthCooldown = 3f;    // ���� ���� �� �ٽ� ���� ������������� ��� �ð�
    //�ð��� ȿ�� ����
    private Renderer playerRenderer;      // �÷��̾��� Renderer ������Ʈ 
    private Color originalColor;          // ���� ���� ���� ���������ÿ��� ����
    public Color hiddenColor = new Color(1, 1, 1, 0.3f); // ���� �� ���� ������ ����

    // ���� ���� �� �ʱ�ȭ
    void Start()
    {
        playerRenderer = GetComponent<Renderer>(); // Renderer ������Ʈ ��������
        originalColor = playerRenderer.material.color;// ���� ���� ����
    }

    // �� �����Ӹ��� ȣ��
    void Update()
    {
        // H Ű�� ������ �� ���� �õ�
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isHidden && canStealth)// ���� �����ϰ� ���� ���� ���� �ƴ϶��
            {
                ToggleStealth();// ���� ����
            }
            else if (isHidden)// ���� ���� ��� �ٽ� ������ ����
            {
                ToggleStealth();// ���� ����
            }
        }

        if (isHidden)// ���� ���̶�� �ð� ����
        {
            stealthTimer += Time.deltaTime;

            // ������ �ִ� ���� �ð� �ʰ� �� �ڵ� ����
            if (stealthTimer >= maxStealthTime)
            {
                Debug.Log("���� �ð��� �������ϴ�.");
                ToggleStealth(); // ���� ����
                StartCoroutine(StealthCooldownRoutine()); // ��Ÿ�� ����
            }
        }
    }
    void ToggleStealth()// ���� ���� ��� (�ѱ�/����)
    {
        isHidden = !isHidden;// ���� ���� ����

        if (isHidden)
        {
            EnterStealth();// ���� ���� ����
        }
        else
        {
            ExitStealth();// ���� ���� ����
        }

        Debug.Log("���� ����: " + isHidden);
    }
    void EnterStealth()// ���� ���� �� ����Ǵ� �Լ�
    {
        SetPlayerAlpha(hiddenColor.a);  // ���� ����
        stealthTimer = 0f;// Ÿ�̸� ����
    }
    void ExitStealth()// ���� ���� �� ����Ǵ� �Լ�
    {
        SetPlayerAlpha(originalColor.a); // ���� ������ ����
    }

    void SetPlayerAlpha(float alpha)// Renderer�� ���� ����(����) �� ����
    {
        Color c = playerRenderer.material.color;
        c.a = alpha;
        playerRenderer.material.color = c;
    }

    public bool IsHidden()// �ܺο��� ���� ���� ���¸� Ȯ���� �� �ֵ��� ����

    {
        return isHidden;
    }

    // ������ ������ �� ���� �ð� ���� �ٽ� ������ �� ������ ��Ÿ���� �����ϴ� �ڷ�ƾ
    private System.Collections.IEnumerator StealthCooldownRoutine()
    {
        canStealth = false;// ���� ��Ȱ��ȭ
        yield return new WaitForSeconds(stealthCooldown); // ��Ÿ�� ���
        canStealth = true; // ���� �ٽ� ����
        Debug.Log("���� ���� ���·� ������");
    }
}

