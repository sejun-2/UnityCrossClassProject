using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCanvas : UICanvas<PopUpCanvas>
{
    [SerializeField] GameObject blocker;

    private Stack<BaseUI> stack = new Stack<BaseUI>();

    private void AddUI(BaseUI ui)
    {
        if (stack.Count > 0)
        {
            BaseUI top = stack.Peek();
            top.gameObject.SetActive(false);
        }
        stack.Push(ui);

        blocker.SetActive(true);
    }

    private void RemoveUI()
    {
        if (stack.Count == 0)
            return;

        BaseUI top = stack.Pop();
        Destroy(top.gameObject);

        if (stack.Count > 0)
        {
            top = stack.Peek();
            top.gameObject.SetActive(true);
        }
        else
        {
            blocker.SetActive(false);
        }
    }

    public T ShowPopUp<T>() where T : BaseUI
    {
        T prefab = Resources.Load<T>($"UI/PopUp/{typeof(T).Name}");
        T instance = Instantiate(prefab, transform);

        AddUI(instance);
        return instance;
    }

    public void ClosePopUp()
    {
        RemoveUI();
    }
}
