using EditorAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// A Component managing the Health of an entity, whether that be Player, Enemy, or Destructible Object.
/// </summary>

public class Health : MonoBehaviour, IDamagable
{
	//Config
	[SerializeField] protected int maxHealth;
	[SerializeField] protected UltEvents.UltEvent<int> damageEvent = new();
    [SerializeField] public UltEvents.UltEvent depleteEvent = new();
    [SerializeField] protected Attack.Tag[] immuneTags;

	//Data
	[SerializeField, HideInEditMode, DisableInPlayMode] protected int health;
	protected bool damagable = true;

	//Getters
	public int GetCurrentHealth() => health;
	public int GetMaxHealth() => maxHealth;
	public float GetHealthPercentage() => health / maxHealth;

	protected virtual void Awake() => health = maxHealth;


    public bool Damage(Attack attack)
    {
        if (!damagable || attack.amount < 1 || immuneTags.IncludesAny(attack.tags) || !OverrideDamageable(attack)) return false;
        OverrideDamageValue(ref attack);

        health -= attack.amount;

		if(health < 0) health = 0;

        OnDamage(attack);
        if (health == 0) OnDeplete(attack);

        return true;
    }

    protected virtual void OnDamage(Attack attack) => damageEvent?.Invoke(attack.amount);
    protected virtual void OnHeal(int amount) {

     }
    protected virtual void OnDeplete(Attack attack) => depleteEvent?.Invoke();

    /// <summary>
    /// Overrides whether this thing can be damaged under certain conditions
    /// </summary>
    /// <param name="attack">The attack fed in.</param>
    /// <returns>Whether the attack successfully connects.</returns>
	protected virtual bool OverrideDamageable(Attack attack) { return true; }
	protected virtual void OverrideDamageValue(ref Attack attack) { }

    public bool Heal(int amount)
	{
		if (amount < 1 || health == maxHealth) return false;

		health += amount;


		if (health > maxHealth) health = maxHealth;
        OnHeal(amount);

        return true;
	}

    //[System.Obsolete]
    //public bool Damage(Attack_Old attack)
    //{
    //    if (!damagable || attack.amount < 1) return false;
    //
    //    if (health - attack.amount < 0) attack.amount = health;
    //
    //    health -= attack.amount;
    //
    //    OnDamage(attack);
    //    if (health == 0) OnDeplete(attack);
    //
    //    return true;
    //}
    //
    //[System.Obsolete] protected virtual void OnDamage(Attack_Old attack) => damageEvent?.Invoke(attack.amount);
    //[System.Obsolete] protected virtual void OnDeplete(Attack_Old attack) => depleteEvent?.Invoke();

}