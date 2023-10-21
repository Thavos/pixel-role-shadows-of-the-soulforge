using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(PlayerMovement))]
public class PlayerPossess : MonoBehaviour
{
    [SerializeField]
    private LayerMask possessLayer;
    private GameObject possess = null;

    [SerializeField]
    private int possessLevel;

    // components that need to be turned off or altered by call when possessing other characters
    private SpriteRenderer sr;
    private CircleCollider2D col;
    private PlayerMovement pm;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D target = new Collider2D();
            target = Physics2D.OverlapCircle(mousePos, 0.5f, possessLayer);

            if (possess != null) // check if we have possessed body
            {
                if (target == null) // check if target is empty and we want to leave the body
                {
                    LeavePossess(); // leave possessed body
                    return;
                }
                else if (possess == target.gameObject) // check if we are targeting ourselves
                {
                    return;
                }
                else if (CheckPossess(target) == true) // check if possess level is high enough
                {
                    SetPossess(target); // set new possess and leave the old one
                }
            }
            else if (target != null) // check if we are targeting something
            {
                if (CheckPossess(target) == true) // check if possess level is high enough
                    SetPossess(target); // set new possess
            }
        }
    }

    private bool CheckPossess(Collider2D target)
    {
        if (possessLevel < target.GetComponent<CharacterPossess>().GetPossessLevel)
        {
            Debug.Log("Possess Level too high");
            return false;
        }

        return true;
    }

    private void SetPossess(Collider2D target)
    {
        if (possess != null)
            possess.GetComponent<CharacterPossess>().SetPossess(false);

        possess = target.gameObject;
        possess.GetComponent<CharacterPossess>().SetPossess(true);

        sr.enabled = false;
        col.enabled = false;
        pm.SetPossess(possess);
    }

    private void LeavePossess()
    {
        possess.GetComponent<CharacterPossess>().SetPossess(false);
        possess = null;

        sr.enabled = true;
        col.enabled = true;
        pm.SetPossess();
    }
}
