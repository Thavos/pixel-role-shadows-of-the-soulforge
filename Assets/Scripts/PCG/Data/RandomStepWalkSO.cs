using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RSW_SO", menuName = "Generators Data SO")]
public class RandomStepWalkSO : ScriptableObject
{
    public int rIterrations, rWalkLength = 0;
    public bool randomiseItteration = false;
}
