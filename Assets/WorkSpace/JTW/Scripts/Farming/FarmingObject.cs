using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingObject : MonoBehaviour, IInteractable
{
    // �׽�Ʈ�� ������
    [SerializeField] private Item _testItem;

    private Inventory _farmingInven;

    private void Awake()
    {
        _farmingInven = new Inventory();

        // �׽�Ʈ�� ������ �߰�.
        _farmingInven.AddItem(_testItem);
    }

    public void Interact()
    {
        Manager.UI.Inven.ShowTradeFarming(_farmingInven);
        Manager.Player.Stats.isFarming = true;
    }
}
