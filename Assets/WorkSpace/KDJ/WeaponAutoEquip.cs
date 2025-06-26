using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 시작 시 자동으로 검 생성 & 장착
public class AutoEquipWeapon : MonoBehaviour
{
    public GameObject weaponPrefab; // 검 프리팹
    public Transform attachPoint;   // 손 위치
    public LayerMask enemyLayer;    // 적 레이어 (Zombie)

    void Start()
    {
        // 검 인스턴스 생성
        GameObject sword = Instantiate(weaponPrefab, attachPoint.position, attachPoint.rotation);
        sword.transform.SetParent(attachPoint);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        // 무기 스크립트 가져오기
        IWeapon weapon = sword.GetComponent<IWeapon>();
        if (weapon is SimpleSword simpleSword)
        {
            simpleSword.targetLayer = enemyLayer;
        }

        // PlayerCombat에 무기 장착
        PlayerCombat combat = GetComponent<PlayerCombat>();
        if (combat != null)
        {
            combat.EquipWeapon(weapon);
            Debug.Log("기본 검 장착 완료");
        }
    }
}

