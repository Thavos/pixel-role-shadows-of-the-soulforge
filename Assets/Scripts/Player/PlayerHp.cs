using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    public RectTransform hpBar;

    [SerializeField]
    private float maxHp, regeneration;
    private float hp, paymentLevel = 2;

    private void Start()
    {
        hp = maxHp;
    }

    private void UpdateBar()
    {
        if (hp < 0)
        {
            Time.timeScale = 0;
            Debug.Log("Player Died");
        }
        else if (hp > maxHp)
            hp = maxHp;

        float Xpos = (1 - (hp / maxHp)) * hpBar.sizeDelta.x;
        hpBar.anchoredPosition = new Vector2(-Xpos, 0);
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        UpdateBar();
    }

    public void PayManaCost()
    {
        float amount = maxHp / paymentLevel;
        hp -= amount;
        UpdateBar();
    }
}
