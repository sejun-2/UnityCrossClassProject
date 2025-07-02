using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeTest : MonoBehaviour
{
    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = transform.GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            FadeOut();
        }
    }

    public void FadeOut()
    {
        foreach (var renderer in _renderers)
        {

            foreach (var mat in renderer.materials)
            {
                Debug.Log(renderer.gameObject.name);
                Color color = mat.color;
                color.a = 0f;
                mat.color = color;
            }
        }
    }
}
