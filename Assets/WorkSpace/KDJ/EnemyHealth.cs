using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ĳ������ ü�� ���� ��ũ��Ʈ
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;// �ִ� ü�� (�ν����Ϳ��� ���� ����)
    private int currentHealth;
    void Start() => currentHealth = maxHealth;// ���� ���� �� �ִ� ü������ �ʱ�ȭ
    public void TakeDamage(int amount)// �������� �޾��� �� ȣ��Ǵ� �Լ�
    {
        currentHealth -= amount;// ü�� ����

        if (currentHealth <= 0)// ü���� 0 ���Ϸ� �������� ��� ó��
        {
            Die();
        }
    }
    void Die()// �� ��� ó�� �Լ�
    {
        Destroy(gameObject);// ���� ������Ʈ�� ������ ���� (�Ǵ� ��� �ִϸ��̼� �� ���� ����)
    }
}
