using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �� ���� �����ϰ� �÷��̾�� ����
public class WeaponAutoEquip : MonoBehaviour
{
    public GameObject weaponPrefab; // �� ������ (SimpleSword�� �پ� �־�� ��)
    public Transform attachPoint;   // �� ��ġ (���� ���� ����)
    public LayerMask enemyLayer;    // �� ���̾� (��: "Zombie")

    void Start()
    {
        // �� ������ �ν��Ͻ� ����
        GameObject sword = Instantiate(weaponPrefab, attachPoint.position, attachPoint.rotation);

        // �� ��ġ�� ����
        sword.transform.SetParent(attachPoint);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        // SimpleSword ������Ʈ ��������
        SimpleSword simpleSword = sword.GetComponent<SimpleSword>();
        if (simpleSword != null)
        {
            // ���� ��� ���̾� ����
            simpleSword.targetLayer = enemyLayer;

            // PlayerCombat�� ����
            PlayerCombat combat = GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.EquipWeapon(simpleSword);
                Debug.Log("�⺻ �� ���� �Ϸ�");
            }
        }
        else
        {
            Debug.LogWarning("SimpleSword ������Ʈ�� �� �����տ��� ã�� �� �����ϴ�.");
        }
    }
}
