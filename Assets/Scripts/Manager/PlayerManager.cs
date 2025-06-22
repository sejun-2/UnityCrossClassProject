using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // TODO : 스탯 초기화 로직 작성
    public PlayerStats Stat = new();

    // 현재 생성된 Player 오브젝트의 Transform
    public Transform Transform;
}
