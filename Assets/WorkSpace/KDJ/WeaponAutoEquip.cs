using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 시작 시 검을 생성하고 플레이어에게 장착
public class WeaponAutoEquip : MonoBehaviour
{
    public GameObject weaponPrefab; // 검 프리팹 (SimpleSword가 붙어 있어야 함)
    public Transform attachPoint;   // 손 위치 (무기 부착 지점)
    public LayerMask enemyLayer;    // 적 레이어 (예: "Zombie")

    void Start()
    {
        // 검 프리팹 인스턴스 생성
        GameObject sword = Instantiate(weaponPrefab, attachPoint.position, attachPoint.rotation);

        // 손 위치에 부착
        sword.transform.SetParent(attachPoint);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        // SimpleSword 컴포넌트 가져오기
        SimpleSword simpleSword = sword.GetComponent<SimpleSword>();
        if (simpleSword != null)
        {
            // 공격 대상 레이어 설정
            simpleSword.targetLayer = enemyLayer;

            // PlayerCombat에 장착
            PlayerCombat combat = GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.EquipWeapon(simpleSword);
                Debug.Log("기본 검 장착 완료");
            }
        }
        else
        {
            Debug.LogWarning("SimpleSword 컴포넌트를 검 프리팹에서 찾을 수 없습니다.");
        }
    }
}
