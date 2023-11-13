using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Abilities/Damage/Projectile", order = 1)]
public class Projectile : Damage
{
    public override void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer)
    {
        base.Cast(parent, position, direction, spellLayer);
    }
}
