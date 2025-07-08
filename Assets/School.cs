using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class School : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("SchoolObject", LoadSceneMode.Additive);
    }
}
