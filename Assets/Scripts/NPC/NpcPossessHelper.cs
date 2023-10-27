using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPossessHelper : MonoBehaviour
{
    private NpcHP hp;
    private NpcMovement movement;

    private void Start()
    {
        hp = GetComponent<NpcHP>();
        movement = GetComponent<NpcMovement>();
    }

    public void SetPossess(PlayerPossessHelper helper)
    {
        if (helper == null) // On null reset state do default behaviour
        {
            gameObject.tag = "Enemy";
            gameObject.layer = (int)NpcLayers.Enemy;
            hp.SetPossess(null);
        }
        else // If helper exists, establish every needed information for player possess helper
        {
            gameObject.tag = "Player";
            gameObject.layer = (int)NpcLayers.Player;
            hp.SetPossess(helper.GetPossessHp());
            helper.SetMovement(movement.GetSpeed, movement.GetRb);
        }
    }
}
