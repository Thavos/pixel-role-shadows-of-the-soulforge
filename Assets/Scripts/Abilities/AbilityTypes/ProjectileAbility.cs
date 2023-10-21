using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageProjectile", menuName = "Abilities/Damage/Projectile", order = 0)]
public class ProjectileAbility : DamageProjectile
{
    public override void Cast(Transform parent, Vector3 position, Vector3 direction, string targetTag)
    {
        base.Cast(parent, position, direction, targetTag);
    }
}
