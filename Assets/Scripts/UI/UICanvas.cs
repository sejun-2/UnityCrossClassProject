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
            if (_instance != null)  // 이미 인스턴스가 존재하면 해당 인스턴스를 반환합니다.
                return _instance;

            _instance = FindObjectOfType<T>();  // 현재 씬에서 T 타입의 인스턴스를 찾습니다.
            if (_instance != null)  // 찾은 인스턴스가 null이 아니면 해당 인스턴스를 반환합니다.
                return _instance;

            T prefab = Resources.Load<T>($"UI/{typeof(T).Name}");   // Resources 폴더에서 T 타입의 프리팹을 로드합니다.
            return Instantiate(prefab);
        }
    }
}
