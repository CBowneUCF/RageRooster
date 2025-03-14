using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : AttackSourceSingle
{
    public bool disableOnHit;
    public bool disableOnlyWithHealth;

    private void OnTriggerEnter(Collider other) => Contact(other.gameObject);
    private void OnCollisionEnter(Collision collision) => Contact(collision.gameObject);


    public override void Contact(GameObject target)
    {
        if (!enabled) return;

        if(target.TryGetComponent(out IDamagable targetDamagable))
        {
            targetDamagable.Damage(GetAttack());
            if (disableOnHit) Disable();
        }
        else
        {
            if (disableOnHit && !disableOnlyWithHealth) Disable();
        }
    }

    public void Disable()
    {
        if (PoolableObject.Is(gameObject, out PoolableObject P)) P.Disable();
        else Destroy(gameObject);
    }
}
