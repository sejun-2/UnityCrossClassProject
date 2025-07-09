using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject linkedDarkness;
    [SerializeField] private GameObject linkedDarkness2;

    private bool _isOpen = false;

    [SerializeField] private GameObject _door;

    [SerializeField] AudioClip audioClip1;
    [SerializeField] AudioClip audioClip2;
    private void Start()
    {
        
    }
    public void Interact()
    {
        if (_door == null) return;

        _isOpen = !_isOpen;
        
        if (_isOpen)
        {
            Debug.Log("¹® ¿­¸²");
            _door.SetActive(false);
            Manager.Sound.SfxPlay(audioClip1, transform, .5f);
            if (linkedDarkness != null)           
                linkedDarkness.SetActive(false);
            if (linkedDarkness2 != null)
                linkedDarkness2.SetActive(false);

        }
        else
        {
            Debug.Log("¹® ´ÝÈû");
            _door.SetActive(true);
            Manager.Sound.SfxPlay(audioClip2, transform, .5f);
        }
    }
}
