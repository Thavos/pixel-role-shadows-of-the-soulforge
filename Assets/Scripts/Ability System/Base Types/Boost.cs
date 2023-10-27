using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : AbilityBase
{
    public readonly LayerMask targetLayers;
    public readonly float hitRadius,
                          effectRadius,
                          castPeriod,
                          duration,
                          effectDuration;
    public readonly GameObject spellPrefab;

    public override void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer)
    {
        GameObject spell = Instantiate(spellPrefab, parent);
        spell.transform.localScale = hitRadius * spell.transform.localScale;
        base.Cast(parent, position, direction, spellLayer);
    }

    public virtual void Apply(Vector3 position) { }
}
