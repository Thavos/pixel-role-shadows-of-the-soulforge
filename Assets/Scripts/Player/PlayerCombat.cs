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
    private PlayerHp pHp;

    private void Start()
    {
        pHp = GetComponent<PlayerHp>();

        for (int i = 0; i < abilities.Count; i++)
        {
            cooldowns.Add(0);
        }
    }

    private void Update()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (Input.GetKey(keyBindings[i]))
            {
                if (abilities[i].CheckCooldown(cooldowns[i]) || abilities[i].manaCost)
                {
                    if (abilities[i].CheckCooldown(cooldowns[i]) == false && abilities[i].manaCost)
                        pHp.PayManaCost();

                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 dir = mousePos - transform.position;
                    dir = dir.normalized;

                    cooldowns[i] = Time.time;
                    abilities[i].Cast(transform, transform.position + dir, dir, (int)SpellLayers.PlayerAbility);
                }
            }
        }
    }
}
