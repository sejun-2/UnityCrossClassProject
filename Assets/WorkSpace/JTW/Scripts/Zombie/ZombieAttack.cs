using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _attackCooldown = 3.9f;
    [SerializeField] private float _attackRange = 2f;

    private Zombie _zombie;
    private Animator _animator;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _animator = GetComponentInChildren<Animator>();
    }


    public void Attack()
    {
        _animator.Play("ZombieAttack");

        StartCoroutine(AttackCoroutine(_attackCooldown));
    }

    public void TryAttack()
    {
        if(Vector3.Distance(transform.position, Manager.Player.Transform.position) <= _attackRange)
        {
            Manager.Player.Transform.GetComponent<IDamageable>().TakeDamage(_damage);
        }
    }

    private IEnumerator AttackCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        if(_zombie.CurrentState != Zombie.State.TakeDamage)
        {
            _zombie.StateChange(Zombie.State.Wait);
        }
    }
}
