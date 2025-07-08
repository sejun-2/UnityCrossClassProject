using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ü�� �� ������ ó�� ��� Ŭ����
public class EnemyStats : MonoBehaviour
{
    public float health = 50f; // ���� ü��

    // �÷��̾� ���ݿ� ���� �������� �޴� �Լ�
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("���� �������� ����! ���� ü��: " + health);

        if (health <= 0f)
        {
            Die(); // ü���� 0 �����̸� ��� ó��
        }
    }

    void Die()
    {
        Debug.Log("���� ����߽��ϴ�.");
        Destroy(gameObject); // �� ���� ������Ʈ ����
    }
}