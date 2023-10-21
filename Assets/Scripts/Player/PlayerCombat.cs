using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private List<KeyCode> keyBindings = new List<KeyCode>();
    [SerializeField]
    private List<AbilityBase> abilities = new List<AbilityBase>();
    private List<float> cooldowns = new List<float>();

    // reference to health system that is used as mana for some spells
    private PlayerHealth pHp;

    private void Start()
    {
        pHp = GetComponent<PlayerHealth>();

        for (int i = 0; i < abilities.Count; i++)
        {
            cooldowns.Add(0);
        }
    }

    private void Update()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (Input.GetKey(keyBindings[i]) && abilities[i].CheckCooldown(cooldowns[i]))
            {
                if (pHp.GetHp - abilities[i].GetManaCost <= 0)
                    continue;

                pHp.TakeDamage(abilities[i].GetManaCost);

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 dir = mousePos - transform.position;
                dir = dir.normalized;

                cooldowns[i] = Time.time;
                abilities[i].Cast(transform, transform.position + dir, dir, "Enemy");
            }
        }
    }
}
