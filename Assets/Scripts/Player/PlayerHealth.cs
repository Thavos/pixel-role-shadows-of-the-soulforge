using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private RectTransform hpBar;
    [SerializeField]
    private int maxHp, regeneration;
    private int hp;
    public int GetHp { get { return hp; } }

    private void Start()
    {
        hp = maxHp;

        UpdateBar();

        if (regeneration != 0)
            StartCoroutine("RegenerateHp");
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (hp < 0)
            hp = 0;

        hpBar.localScale = new Vector3((float)hp / maxHp, 1, 1);
    }

    IEnumerator RegenerateHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            hp += regeneration;
            UpdateBar();
        }
    }
}
