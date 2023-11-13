using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "Abilities/Boost/SpeedBoost", order = 1)]
public class SpeedBoost : Boost
{
    [SerializeField]
    private float speedMultiplayer;

    private LayerMask targetLayer;

    public override void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer)
    {
        switch (spellLayer)
        {
            case (int)SpellLayers.EnemyAbility:
                targetLayer = (1 << (int)NpcLayers.Enemy);
                break;
            case (int)SpellLayers.PlayerAbility:
                targetLayer = (1 << (int)NpcLayers.Player);
                break;
            default:
                targetLayer = (1 << (int)NpcLayers.Player) |
                              (1 << (int)NpcLayers.Golem) |
                              (1 << (int)NpcLayers.Enemy) |
                              (1 << (int)NpcLayers.NPC);
                break;
        }
        base.Cast(parent, position, direction, spellLayer);
    }

    public override void Apply(Vector3 position)
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(position, hitRadius, targetLayer);
        foreach (Collider2D target in collisions)
        {
            target.gameObject.SendMessage("BoostSpeed", speedMultiplayer, SendMessageOptions.DontRequireReceiver);
        }
        base.Apply(position);
    }

    public override void Cancel(Vector3 position)
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(position, hitRadius, targetLayer);
        foreach (Collider2D target in collisions)
        {
            target.gameObject.SendMessage("BoostSpeed", 1, SendMessageOptions.DontRequireReceiver);
        }
        base.Cancel(position);
    }
}
