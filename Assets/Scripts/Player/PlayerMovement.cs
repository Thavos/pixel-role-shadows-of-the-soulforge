using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private int speed;
    private Vector2 dirVector;
    private Rigidbody2D rb;
    private bool isPossessing = false;
    private const float diagonalFix = 0.70710678118f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dirVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (dirVector.x != 0 && dirVector.y != 0)
        {
            dirVector.x = Mathf.Clamp(dirVector.x, -diagonalFix, diagonalFix);
            dirVector.y = Mathf.Clamp(dirVector.y, -diagonalFix, diagonalFix);
        }

        if (isPossessing)
            transform.position = rb.transform.position;
    }

    private void FixedUpdate()
    {
        rb.velocity = dirVector * speed * Time.fixedDeltaTime;
    }

    public void SetPossess(GameObject possess)
    {
        isPossessing = true;
        rb = possess.GetComponent<Rigidbody2D>();
    }

    public void SetPossess()
    {
        isPossessing = false;
        rb = GetComponent<Rigidbody2D>();
    }
}
