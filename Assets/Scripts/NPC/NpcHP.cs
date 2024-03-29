using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHP : MonoBehaviour
{
    [SerializeField]
    private float maxHp, regeneration;
    private float hp;
    private PlayerPossessHp possessHp; // also functions as a checker value if character is possessed 

    private void Start()
    {
        possessHp = null;
        hp = maxHp;
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (possessHp != null)
            possessHp.UpdateBar(hp);
    }

    public void SetPossess(PlayerPossessHp possessHp)
    {
        this.possessHp = possessHp;
        if (possessHp != null)
            possessHp.SetPossess(maxHp, hp, regeneration);
    }
}
