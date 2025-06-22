using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas<T> : MonoBehaviour where T : MonoBehaviour
{
    private T _instance;
    public T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<T>();
            if (_instance != null)
                return _instance;

            T prefab = Resources.Load<T>($"UI/{typeof(T).Name}");
            return Instantiate(prefab);
        }
    }
}
