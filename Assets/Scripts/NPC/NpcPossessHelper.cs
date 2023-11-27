using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPossessHelper : MonoBehaviour
{
    [SerializeField]
    private NpcLayers originLayer;

    private NpcHP hp;
    private NpcMovement movement;
    private NpcCombat abilities;

    private void Start()
    {
        hp = GetComponent<NpcHP>();
        movement = GetComponent<NpcMovement>();
        abilities = GetComponent<NpcCombat>();

        if (originLayer == NpcLayers.Enemy)
        {
            gameObject.GetComponent<EnemyFollow>().Speed = movement.GetSpeed; ;
        }
    }

    public void SetPossess(PlayerPossessHelper helper)
    {
        if (helper == null) // On null reset state do default behaviour
        {
            if (originLayer == NpcLayers.Golem) // Easy way to make sure we set previous layer and tag to possessable character
            {
                gameObject.tag = "Golem";
                gameObject.layer = (int)NpcLayers.Golem;
            }
            else if (originLayer == NpcLayers.Enemy)
            {
                gameObject.tag = "Enemy";
                gameObject.layer = (int)NpcLayers.Enemy;
                gameObject.GetComponent<EnemyFollow>().Possessed = false;
                gameObject.GetComponent<EnemyCombat>().Possessed = false;
            }
            else
            {
                gameObject.tag = "NPC";
                gameObject.layer = (int)NpcLayers.NPC;
            }

            hp.SetPossess(null);
            abilities.SetPossess(false);
        }
        else // If helper exists, establish every needed information for player possess helper
        {
            gameObject.tag = "Player";
            gameObject.layer = (int)NpcLayers.Player;

            hp.SetPossess(helper.GetPossessHp());
            abilities.SetPossess(true);

            if (originLayer == NpcLayers.Enemy)
            {
                gameObject.GetComponent<EnemyFollow>().Possessed = true;
                gameObject.GetComponent<EnemyCombat>().Possessed = true;
            }

            helper.SetMovement(movement.GetSpeed, movement.GetRb);
            helper.SetAbilities(abilities.GetAbilities, abilities.GetAbilityPoint);
        }
    }
}
