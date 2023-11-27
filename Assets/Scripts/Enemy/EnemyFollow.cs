using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField]
    private float visionRange, distance;

    private Rigidbody2D rb;
    private Vector3 lastPosition;
    LayerMask mask;

    private float speed;
    public float Speed { set { speed = value; } get { return speed; } }

    private bool possessed;
    public bool Possessed { set { possessed = value; } get { return possessed; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        possessed = false;
        lastPosition = transform.position;
        mask = (1 << (int)NpcLayers.Player) |
               (1 << (int)ObjectLayers.Wall);
    }

    private void FixedUpdate()
    {
        if (possessed == false)
        {
            Transform player = FindPlayer();
            if (player != null)
            {
                lastPosition = player.position;
                MoveToPos(lastPosition);
            }
            else if (lastPosition != transform.position)
            {
                MoveToPos(lastPosition);
            }
        }
    }

    private void MoveToPos(Vector3 position)
    {
        if (Vector2.Distance(position, transform.position) > distance)
        {
            Vector3 dir = position - transform.position;
            dir.Normalize();
            rb.velocity = dir * speed * Time.fixedDeltaTime;
            //transform.position += dir * 0.01f;
        }
    }

    public Transform FindPlayer()
    {
        Transform player = null;
        RaycastHit2D ray = new RaycastHit2D();
        RaycastHit2D circleRay = Physics2D.CircleCast(transform.position, visionRange, Vector2.right, visionRange, (1 << (int)NpcLayers.Player));

        if (circleRay.collider != null)
        {
            Vector2 dir = circleRay.transform.position - transform.position;
            dir.Normalize();
            ray = Physics2D.Raycast(transform.position, dir, visionRange, mask);
            if (ray.collider != null && ray.collider.CompareTag("Player"))
            {
                player = ray.collider.transform;
            }
        }

        return player;
    }
}
