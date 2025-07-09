using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private Sprite _tutorialSprite;
    private List<GameObject> _tutorialUIs = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _tutorialUIs.Add(Manager.UI.Inven.ShowTutorialUI(transform, _tutorialSprite));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in _tutorialUIs)
            {
                Destroy(obj);
            }
        }
    }
}
