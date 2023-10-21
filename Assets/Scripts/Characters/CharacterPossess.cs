using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPossess : MonoBehaviour
{
    [SerializeField]
    private int possessLevel;
    public int GetPossessLevel { get { return possessLevel; } }

    public void SetPossess(bool value)
    {
        Debug.Log(value);
    }
}
