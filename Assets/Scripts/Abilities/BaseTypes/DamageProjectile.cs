using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectile : AbilityBase
{
    [SerializeField]
    private int damage;
    public float GetDamage { get { return damage; } }
    [SerializeField]
    private float projectileVelocity;
    public float GetVelocity { get { return projectileVelocity; } }
    [SerializeField]
    public GameObject spellPrefab;

    public override void Cast(Transform parent, Vector3 position, Vector3 direction, string targetTag)
    {
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        GameObject spell = Instantiate(spellPrefab, position, rotation);
        spell.GetComponent<DamageAbilityConroller>().SetTarget(targetTag);
    }

    public virtual void OnHit(GameObject target)
    {
        target.SendMessage("TakeDamage", damage);
    }
}
