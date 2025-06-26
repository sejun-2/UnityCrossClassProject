using UnityEngine;

public class ItemBox : MonoBehaviour, IInteractable
{
    // �׽�Ʈ�� ������
    [SerializeField] private Item _testItem;

    private Inventory _farmingInven;

    [SerializeField] private PlayerInteraction _interaction;

    private void Awake()
    {
        _farmingInven = new Inventory();

        // �׽�Ʈ�� ������ �߰�.
        _farmingInven.AddItem(_testItem);
    }

    public void Interact()
    {
        Manager.UI.Inven.ShowTradeFarming(_farmingInven);
        _interaction.isFarming = true;
    }
}
