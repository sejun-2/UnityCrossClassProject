using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �� �ڵ����� �� ���� & ����
public class AutoEquipWeapon : MonoBehaviour
{
    public GameObject weaponPrefab; // �� ������
    public Transform attachPoint;   // �� ��ġ
    public LayerMask enemyLayer;    // �� ���̾� (Zombie)

    void Start()
    {
        // �� �ν��Ͻ� ����
        GameObject sword = Instantiate(weaponPrefab, attachPoint.position, attachPoint.rotation);
        sword.transform.SetParent(attachPoint);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        // ���� ��ũ��Ʈ ��������
        IWeapon weapon = sword.GetComponent<IWeapon>();
        if (weapon is SimpleSword simpleSword)
        {
            simpleSword.targetLayer = enemyLayer;
        }

        // PlayerCombat�� ���� ����
        PlayerCombat combat = GetComponent<PlayerCombat>();
        if (combat != null)
        {
            combat.EquipWeapon(weapon);
            Debug.Log("�⺻ �� ���� �Ϸ�");
        }
    }
}

