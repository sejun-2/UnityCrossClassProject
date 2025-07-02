using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public partial class PlayerStats
{
    public Stat<bool> IsAttack = new();
}

public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        Item weapon = Manager.Player.Stats.Weapon.Value;

        if (weapon == null || Manager.Player.Stats.IsAttack.Value) return;

        _animator.Play("BatAttack");

        Manager.Player.Stats.IsAttack.Value = true;
        StartCoroutine(AttackCoroutine(weapon.attackSpeed));
    }

    private void TryAttack()
    {
        Debug.Log("공ㅇㅇㅇㅇ겨ㅕㅕㅕㅕ시ㅣㅣㅣ도ㅗ이ㅓㅣ");

        Item weapon = Manager.Player.Stats.Weapon.Value;

        Vector3 direction = new Vector3(transform.localScale.x, 0, 0);

        Debug.DrawRay(transform.position + Vector3.up, direction * weapon.attackRange * 10, Color.red, 1f);
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, direction, weapon.attackRange * 10, ~0, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Zombie"))
            {
                IDamageable zombie = hit.collider.gameObject.GetComponent<IDamageable>();

                zombie.TakeDamage(weapon.attackValue);

                weapon.durabilityValue--;

                Debug.Log($"{hit.collider.gameObject.name}에게 {weapon.attackValue} 만큼의 데미지");

                break;
            }
        }
    }

    private IEnumerator AttackCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        Manager.Player.Stats.IsAttack.Value = false;
    }
}
