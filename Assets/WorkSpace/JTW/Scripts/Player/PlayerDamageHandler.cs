using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStats
{
    public Stat<bool> IsTakeDamage = new();

    internal void ReduceFatigue(float v)
    {
    }
}

public class PlayerDamageHandler : MonoBehaviour, IDamageable
{
    [SerializeField] AudioClip _hitSound;

    private PlayerStats Stats => Manager.Player.Stats;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (Stats.CurHp.Value <= 0) return;

        Manager.Player.Stats.IsTakeDamage.Value = true;
        StartCoroutine(TakeDamageCoroutine());
        _animator.Play("BatTakeDamage");
        Manager.Sound.SfxPlay(_hitSound, transform);

        if (Random.value < 0.1)
        {
            Manager.Player.Stats.Buff.Value = PlayerBuffs.Fear;
        }

        if (Stats.Armor.Value != null)
        {
            amount = Mathf.Max(0, amount - Stats.Armor.Value.defValue);
            Stats.Armor.Value.durabilityValue--;
        }

        Stats.ChangeHp(-(amount * Manager.Player.BuffStats.VulnerableDebuff));

        if(Stats.CurHp.Value <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Manager.Game.GameStart();
    }

    public IEnumerator TakeDamageCoroutine()
    {
        yield return new WaitForSeconds(1f);

        Manager.Player.Stats.IsTakeDamage.Value = false;
    }
}
