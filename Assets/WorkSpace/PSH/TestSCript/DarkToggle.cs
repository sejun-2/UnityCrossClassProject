using UnityEngine;

public class DarkToggle : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleDarkObjects();
        }
    }

    void ToggleDarkObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true); // true = 비활성화된 오브젝트도 포함

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "Dark")
            {
                bool isActive = obj.activeSelf;
                obj.SetActive(!isActive);
                Debug.Log(obj.name + " 상태 변경: " + (!isActive));
            }
        }
    }
}
