using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    [SerializeField]
    private int cooldown;
    [SerializeField]
    private int manaCost;
    public int GetManaCost { get { return manaCost; } }

    public virtual void Cast(Transform parent, Vector3 position, Vector3 direction, string targetTag) { }

    public bool CheckCooldown(float lastcast)
    {
        return ((Time.time - lastcast) > cooldown);
    }
}
