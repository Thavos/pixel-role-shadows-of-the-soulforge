using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour
{
    public readonly Boost spell;

    private Coroutine coroutine;
    private float time;

    void Start()
    {
        time = Time.time;
    }

    private void Update()
    {
        if (Time.time - time > spell.duration)
        {
            Cancel();
        }
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
