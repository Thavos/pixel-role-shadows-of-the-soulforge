using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FireBall", menuName = "Abilities/Damage/FireBall", order = 1)]
public class FireBall : Damage
{
    public override void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer)
    {
        base.Cast(parent, position, direction, spellLayer);
    }
}
