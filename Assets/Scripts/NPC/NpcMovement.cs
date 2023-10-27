using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public float GetSpeed { get { return speed; } }

    private Rigidbody2D rb;
    public Rigidbody2D GetRb { get { return rb; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
