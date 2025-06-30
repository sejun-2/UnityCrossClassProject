using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject linkedDarkness;

    private bool _isOpen = false;

    [SerializeField] private GameObject _door;

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

            if (linkedDarkness != null)           
                linkedDarkness.SetActive(false);

        }
        else
        {
            Debug.Log("¹® ´ÝÈû");
            _door.SetActive(true);
        }
    }
}
