using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbilityConroller : MonoBehaviour
{
    [SerializeField]
    private DamageProjectile spell;
    private Rigidbody2D rb;
    private string targetTag;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * (float)(Mathf.Pow(spell.GetVelocity, 2) * 0.5));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            spell.OnHit(other.gameObject);
            Destroy(gameObject);
        }
    }

    public void SetTarget(string target)
    {
        targetTag = target;
    }
}
