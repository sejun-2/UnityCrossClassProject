using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private Dictionary<string, GameObject> gameObjectDic;   // ���ӿ�����Ʈ�� �̸����� ã�� ���� ��ųʸ�
    private Dictionary<string, Component> componentDic; // ������Ʈ�� �̸��� Ÿ������ ã�� ���� ��ųʸ�

    private void Awake()
    {
        // BaseUI�� �������� ��� �ڽ� ���ӿ�����Ʈ�� ������Ʈ�� �������� (��Ȱ��ȭ�� ������Ʈ�� ����)
        RectTransform[] transforms = GetComponentsInChildren<RectTransform>(true);  // GetComponents ��� ������Ʈ�� ����������, RectTransform�� UI ��ҿ��� �ش�ǹǷ� ������ ���� RectTransform�� ����մϴ�.
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length * 4);  // transforms.Length * 4�� �ʱ� �뷮�� �����Ͽ� ������ ����ŵ�ϴ�.
        foreach (RectTransform child in transforms) // RectTransform�� ����Ͽ� UI ����� �ڽ� ������Ʈ�� �����ɴϴ�.
        {
            gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);  // ���ӿ�����Ʈ�� �̸��� Ű�� ����Ͽ� ��ųʸ��� �߰��մϴ�.
        }

        Component[] components = GetComponentsInChildren<Component>(true);  // GetComponentsInChildren ��� ������Ʈ�� ����������, ��Ȱ��ȭ�� ������Ʈ�� ���ԵǹǷ� true�� ����մϴ�.
        componentDic = new Dictionary<string, Component>(components.Length * 4);
        foreach (Component child in components)
        {
            componentDic.TryAdd($"{child.gameObject.name}_{child.GetType().Name}", child);
        }
    }

    public GameObject GetUI(in string name) // ���ӿ�����Ʈ�� �̸����� �������� �޼���
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject); // ��ųʸ����� �̸����� ���ӿ�����Ʈ�� ã���ϴ�.
        return gameObject;  // ���ӿ�����Ʈ�� �����ϸ� ��ȯ�ϰ�, ������ null�� ��ȯ�մϴ�.
    }

    public T GetUI<T>(in string name) where T : Component   // ���׸� �޼����, Ư�� Ÿ���� ������Ʈ�� �̸����� �������� �޼���
    {
        componentDic.TryGetValue($"{name}_{typeof(T).Name}", out Component component);  // ��ųʸ����� �̸��� Ÿ���� �����Ͽ� ������Ʈ�� ã���ϴ�.
        if (component != null)
            return component as T;  // ĳ�õ� ������Ʈ�� �����ϸ� �ش� Ÿ������ ĳ�����Ͽ� ��ȯ�մϴ�.

        GameObject gameObject = GetUI(name);    // ���ӿ�����Ʈ�� �̸����� �����ɴϴ�.
        if (gameObject == null)
            return null;

        component = gameObject.GetComponent<T>();   // ���ӿ�����Ʈ���� �ش� Ÿ���� ������Ʈ�� �����ɴϴ�.
        if (component == null)
            return null;

        componentDic.TryAdd($"{name}_{typeof(T).Name}", component); // ��ųʸ��� ���� ã�� ������Ʈ�� �߰��մϴ�.
        return component as T;
    }

    public PointerHandler GetEvent(in string name)  // PointerHandler ������Ʈ�� �̸����� �������� �޼���
    {
        GameObject gameObject = GetUI(name);    // ���ӿ�����Ʈ�� �̸����� �����ɴϴ�.
        return gameObject.GetOrAddComponent<PointerHandler>();  // ���ӿ�����Ʈ���� PointerHandler ������Ʈ�� �������ų� �߰��մϴ�.
    }
}
