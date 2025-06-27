using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // <�� ��ȯ>
    // ������Ʈ�� ���Ե� �ٸ� ���� �ε��ϰ� ������ ���� ������ ������
    public void ChageScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    // <�� �߰�>
    // ������Ʈ�� ���Ե� �ٸ� ���� �ε��ϰ� ������ ���� ������ ������
    public void AddScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }


}
