using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject effectObj;
    [SerializeField]
    private Collider2D[] colliders;
    private SpriteRenderer sprite;
    private PlayerPossessHp possessHp;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        possessHp = GetComponent<PlayerPossessHp>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public void SetPossess(bool status)
    {
        sprite.enabled = !status;         // Enable / disable player sprite
        effectObj.SetActive(!status);

        foreach (var col in colliders)    // Enable / disable player colliders
            col.enabled = !status;

        if (!status)                      // On false - reset player movement
        {
            playerMovement.SetPossess();
            playerCombat.SetPossess();
            possessHp.SetPossess(false);
        }
    }

    // Contacting possess object changes based upon status value
    //   true  - tell possessing obj we are gettin control of him
    //   false - tell possessing obj we are letting it and it can return to default behaviour
    public void ContactPossess(GameObject obj, bool status)
    {
        NpcPossessHelper helper = null;
        helper = obj.GetComponent<NpcPossessHelper>();

        if (helper != null)
            helper.SetPossess(status == true ? this : null);
    }

    public void SetMovement(float speed, Rigidbody2D rb)
    {
        playerMovement.SetPossess(speed, rb);
    }

    public PlayerPossessHp GetPossessHp()
    {
        return possessHp;
    }

    public void SetAbilities(List<AbilityBase> abilities)
    {
        playerCombat.SetPossess(abilities);
    }
}
