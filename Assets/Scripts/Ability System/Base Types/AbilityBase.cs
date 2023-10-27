using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : ScriptableObject
{
    [SerializeField]
    private int cooldown;

    public readonly bool manaCost;

    public virtual void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer) { }

    public bool CheckCooldown(float lastTimeCasted)
    {
        return ((Time.time - lastTimeCasted) > cooldown);
    }
}
