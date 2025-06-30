using UnityEngine;

public class Hideout : MonoBehaviour,IInteractable
{
    private GameObject _player;


    public void Interact()
    {
        Manager.Player.Stats.isHiding = !Manager.Player.Stats.isHiding;

        if (Manager.Player.Stats.isHiding)
        {
            Debug.Log("¼ûÀ½");
        }
        else
        {
            Debug.Log("¾È¼ûÀ½");

        }
    }

}
