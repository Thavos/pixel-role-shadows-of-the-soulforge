using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessSystem : MonoBehaviour
{
    [SerializeField]
    private LayerMask possessLayer;
    private GameObject possessObj = null;
    private PlayerPossessHelper possessHelper;

    private float setLeavePossessTime = 0.5f,
                  leavePossessTime;

    private void Start()
    {
        possessHelper = GetComponent<PlayerPossessHelper>();

        leavePossessTime = setLeavePossessTime;
    }

    private void Update()
    {
        // Player is trying to possess
        if (Input.GetKeyUp(KeyCode.Space) && leavePossessTime > 0f)
        {
            leavePossessTime = setLeavePossessTime;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D target = null;
            target = Physics2D.OverlapCircle(mousePos, 0.5f, possessLayer);

            if (target != null)
                if (possessObj == null)
                    EnterPossess(target.gameObject);
                else
                    ChangePossess(target.gameObject);
        }
        // Player is trying to leave possess
        else if (possessObj != null)
        {
            if (Input.GetKey(KeyCode.Space) && leavePossessTime > 0f)
                leavePossessTime -= Time.deltaTime;
            else if (Input.GetKey(KeyCode.Space) && leavePossessTime <= 0f)
            {
                leavePossessTime = setLeavePossessTime;
                LeavePossess();
            }
        }
    }

    private void EnterPossess(GameObject possessObj)
    {
        this.possessObj = possessObj;

        possessHelper.SetPossess(true); // Setting internal possessing status
        possessHelper.ContactPossess(possessObj, true); // Entering new possess body
    }

    private void ChangePossess(GameObject possessObj)
    {
        possessHelper.ContactPossess(this.possessObj, false); // Leaving old possess body

        this.possessObj = possessObj;
        possessHelper.ContactPossess(possessObj, true); // Entering new possess body
    }

    private void LeavePossess()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;
        dir = dir.normalized;

        // Check if we hit a wall on our mouse path, if yes, teleport to sgort distance
        //   before the wall, otherwise tepeport directly to mouse position
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dir, 10f, (int)ObjectLayers.Wall);

        float hitDistance = (rayHit.point - (Vector2)transform.position).magnitude;
        float mouseDistance = (mousePos - (Vector2)transform.position).magnitude;

        if (rayHit)
            if (mouseDistance < hitDistance)
                transform.position = mousePos;
            else
                transform.position = rayHit.point - dir * new Vector2(0.2f, 0.2f);
        else
            transform.position = mousePos;

        possessHelper.SetPossess(false); // Setting internal possessing status
        possessHelper.ContactPossess(possessObj, false); // Leaving old possess body
        possessObj = null;
    }

}
