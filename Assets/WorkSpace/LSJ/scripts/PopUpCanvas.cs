using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCanvas : UICanvas<PopUpCanvas>
{
    [SerializeField] GameObject blocker;    // 블로커 오브젝트, 팝업이 열릴 때 활성화되어 다른 UI를 클릭할 수 없도록 하는 오브젝트.

    private Stack<BaseUI> stack = new Stack<BaseUI>();  // 팝업 UI를 관리하기 위한 스택

    private void AddUI(BaseUI ui)   // 팝업 UI를 스택에 추가하고 활성화합니다
    {
        if (stack.Count > 0)    // 스택에 이미 UI가 있는 경우
        {
            BaseUI top = stack.Peek();  // 스택의 맨 위 UI를 가져옵니다
            top.gameObject.SetActive(false);    // 가져온 UI를 비활성화합니다
        }
        else
        {
            Manager.Player.Stats.IsControl.Value = true;
        }
            stack.Push(ui); // 새로운 UI를 스택에 추가합니다

        blocker.SetActive(true);    // 블로커를 활성화하여 다른 UI를 클릭할 수 없도록 합니다
    }

    private void RemoveUI() // 팝업 UI를 스택에서 제거하고 비활성화합니다
    {
        if (stack.Count == 0)   // 스택이 비어있으면 아무 작업도 하지 않습니다
            return;

        BaseUI top = stack.Pop();   // 스택에서 가장 위의 UI를 불러옵니다.
        Destroy(top.gameObject);    // 불러온 가장 최신 UI의 게임 오브젝트를 파괴합니다.

        if (stack.Count > 0)    // 스택에 여전히 UI가 남아있다면
        {
            top = stack.Peek(); // 스택의 맨 위 UI를 가져옵니다
            top.gameObject.SetActive(true); // 가져온 최신의 UI의 게임오브젝트를 다시 활성화합니다.
        }
        else
        {
            Manager.Player.Stats.IsControl.Value = false;
            blocker.SetActive(false);   // 스택이 비어있으면 블로커를 비활성화합니다
        }
    }

    public T ShowPopUp<T>() where T : BaseUI    // BaseUI를 상속받은 UI를 팝업으로 표시하는 메서드입니다.
    {
        // typeof(T).Name을 사용하여 T의 이름을 가져오고, 해당 이름으로 Resources 폴더에서 UI 프리팹을 로드합니다.
        T prefab = Resources.Load<T>($"UI/PopUp/{typeof(T).Name}");
        T instance = Instantiate(prefab, transform);

        AddUI(instance);    // 팝업 UI를 스택에 추가하고 활성화합니다.
        return instance;
    }

    public void ClosePopUp()    // 팝업 UI를 닫는 메서드입니다.
    {
        RemoveUI();
    }
}
