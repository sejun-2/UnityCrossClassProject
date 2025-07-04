using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDamageHandler : MonoBehaviour, IDamageable
{
    private Zombie _zombie;
    private Animator _animator;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (_zombie.CurrentState == Zombie.State.Dead) return;

        _zombie.StateChange(Zombie.State.TakeDamage);

        StartCoroutine(TakeDamageCoroutine());
        _animator.Play("ZombieTakeDamage");

        _zombie.Health -= amount;
        if (_zombie.Health <= 0)
        {
            _zombie.StateChange(Zombie.State.Dead);
        }
    }

    public IEnumerator TakeDamageCoroutine()
    {
        yield return new WaitForSeconds(1f);

        if (_zombie.CurrentState != Zombie.State.Dead)
        {
            _zombie.StateChange(Zombie.State.Wait);
        }
    }
}
