using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopUpPresenter : MonoBehaviour
{
    [SerializeField] private AudioClip _popUpSound;

    private void Start()
    {
        Manager.Player.Stats.isFarming = true;
        Manager.Sound.SfxPlay(_popUpSound, Camera.main.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Player.Stats.isFarming = false;
            Manager.Sound.SfxPlay(_popUpSound, Camera.main.transform);
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.Sound.SfxPlay(_popUpSound, Camera.main.transform);

            if (Manager.Player.Stats.Hunger.Value <= 40
            || Manager.Player.Stats.Thirst.Value <= 30
            || Manager.Player.Stats.Mentality.Value <= 15)
            {
                Manager.Game.ChangeScene("GameOverScene");
            }
            else
            {
                Manager.Game.DayComplete();
                Manager.Game.ChangeScene("BaseCamp");
            }
        }
    }
}
