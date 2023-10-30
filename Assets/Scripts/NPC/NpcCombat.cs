using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCombat : MonoBehaviour
{
    [SerializeField]
    private List<AbilityBase> abilities;
    private bool possessed = false;

    public void SetPossess(bool status)
    {
        possessed = status;
    }

    public List<AbilityBase> GetAbilities()
    {
        return abilities;
    }
}
