using UnityEngine;

public class ItemBox : MonoBehaviour, IInteractable
{
    // 테스트용 아이템
    [SerializeField] private Item _testItem;

    private Inventory _farmingInven;
    
    private void Awake()
    {
        _farmingInven = new Inventory();

        // 테스트용 아이템 추가.
        _farmingInven.AddItem(_testItem);
    }

    public void Interact()
    {
        Manager.UI.Inven.ShowTradeFarming(_farmingInven);
        Manager.Player.Stats.isFarming = true;
    }
}
