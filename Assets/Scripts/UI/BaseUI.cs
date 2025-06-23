using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private Dictionary<string, GameObject> gameObjectDic;   // 게임오브젝트를 이름으로 찾기 위한 딕셔너리
    private Dictionary<string, Component> componentDic; // 컴포넌트를 이름과 타입으로 찾기 위한 딕셔너리

    private void Awake()
    {
        // BaseUI를 기준으로 모든 자식 게임오브젝트의 컴포넌트를 가져오기 (비활성화된 오브젝트도 포함)
        RectTransform[] transforms = GetComponentsInChildren<RectTransform>(true);  // GetComponents 모든 컴포넌트를 가져오지만, RectTransform은 UI 요소에만 해당되므로 성능을 위해 RectTransform을 사용합니다.
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length * 4);  // transforms.Length * 4는 초기 용량을 설정하여 성능을 향상시킵니다.
        foreach (RectTransform child in transforms) // RectTransform을 사용하여 UI 요소의 자식 오브젝트를 가져옵니다.
        {
            gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);  // 게임오브젝트의 이름을 키로 사용하여 딕셔너리에 추가합니다.
        }

        Component[] components = GetComponentsInChildren<Component>(true);  // GetComponentsInChildren 모든 컴포넌트를 가져오지만, 비활성화된 오브젝트도 포함되므로 true를 사용합니다.
        componentDic = new Dictionary<string, Component>(components.Length * 4);
        foreach (Component child in components)
        {
            componentDic.TryAdd($"{child.gameObject.name}_{child.GetType().Name}", child);
        }
    }

    public GameObject GetUI(in string name) // 게임오브젝트를 이름으로 가져오는 메서드
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject); // 딕셔너리에서 이름으로 게임오브젝트를 찾습니다.
        return gameObject;  // 게임오브젝트가 존재하면 반환하고, 없으면 null을 반환합니다.
    }

    public T GetUI<T>(in string name) where T : Component   // 제네릭 메서드로, 특정 타입의 컴포넌트를 이름으로 가져오는 메서드
    {
        componentDic.TryGetValue($"{name}_{typeof(T).Name}", out Component component);  // 딕셔너리에서 이름과 타입을 조합하여 컴포넌트를 찾습니다.
        if (component != null)
            return component as T;  // 캐시된 컴포넌트가 존재하면 해당 타입으로 캐스팅하여 반환합니다.

        GameObject gameObject = GetUI(name);    // 게임오브젝트를 이름으로 가져옵니다.
        if (gameObject == null)
            return null;

        component = gameObject.GetComponent<T>();   // 게임오브젝트에서 해당 타입의 컴포넌트를 가져옵니다.
        if (component == null)
            return null;

        componentDic.TryAdd($"{name}_{typeof(T).Name}", component); // 딕셔너리에 새로 찾은 컴포넌트를 추가합니다.
        return component as T;
    }

    public PointerHandler GetEvent(in string name)  // PointerHandler 컴포넌트를 이름으로 가져오는 메서드
    {
        GameObject gameObject = GetUI(name);    // 게임오브젝트를 이름으로 가져옵니다.
        return gameObject.GetOrAddComponent<PointerHandler>();  // 게임오브젝트에서 PointerHandler 컴포넌트를 가져오거나 추가합니다.
    }
}
