using UnityEngine;
using TMPro;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    public void SetMessage(string msg)
    {
        messageText.text = msg;
    }
}
