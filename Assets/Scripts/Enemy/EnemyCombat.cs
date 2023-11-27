using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField]
    protected float distance;

    protected NpcCombat npcCombat;
    protected EnemyFollow enemyFollow;

    protected List<AbilityBase> abilities = new List<AbilityBase>();
    protected List<float> cooldowns = new List<float>();

    protected Transform abilityPoint;

    protected bool possessed;
    public bool Possessed { set { possessed = value; } get { return possessed; } }
}
