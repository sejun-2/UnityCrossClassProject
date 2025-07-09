using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZombieAnimation : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(StartAnim(Random.value * 3));
    }

    private IEnumerator StartAnim(float time)
    {
        yield return new WaitForSeconds(time);

        _animator.Play("Stay");
    }
}
