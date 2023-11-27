using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    private float time;

    void Start()
    {
        Destroy(gameObject, time);
    }
}
