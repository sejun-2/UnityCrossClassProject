using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // TODO : ���� �ʱ�ȭ ���� �ۼ�
    public PlayerStats Stat = new();

    // ���� ������ Player ������Ʈ�� Transform
    public Transform Transform;
}
