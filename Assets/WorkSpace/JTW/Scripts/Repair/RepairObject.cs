using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string _objectId;
    public string ObjectId => _objectId;

    private void Awake()
    {
        //if (Manager.Game.IsRepairObject[_objectId])
        //{
        //    Repair();
        //}
    }

    public void Interact()
    {
        Manager.UI.Inven.ShowRepairUI(this);
        Manager.Player.Stats.isFarming = true;
    }

    public void Repair()
    {
        GameObject obj = Resources.Load<GameObject>($"Object/{_objectId}");

        Instantiate(obj, transform.position, Quaternion.identity);
    }
}
