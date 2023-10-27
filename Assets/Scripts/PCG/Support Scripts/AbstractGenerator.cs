using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapGenerator tilemapGen;
    [SerializeField]
    protected float seed = 0;
    protected static System.Random rand;

    private void Awake()
    {
        if (seed == 0)
        {
            rand = new System.Random(Random.Range(0, 999999999));
        }
        else
        {
            rand = new System.Random(seed.GetHashCode());
        }
    }

    public void SetRandom()
    {
        if (seed == 0)
        {
            rand = new System.Random(Random.Range(0, 999999999));
        }
        else
        {
            rand = new System.Random(seed.GetHashCode());
        }
    }

    public abstract void Generate();
}
