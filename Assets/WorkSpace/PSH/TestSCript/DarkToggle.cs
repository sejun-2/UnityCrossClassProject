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
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true); // true = ��Ȱ��ȭ�� ������Ʈ�� ����

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "Dark")
            {
                bool isActive = obj.activeSelf;
                obj.SetActive(!isActive);
                Debug.Log(obj.name + " ���� ����: " + (!isActive));
            }
        }
    }
}
