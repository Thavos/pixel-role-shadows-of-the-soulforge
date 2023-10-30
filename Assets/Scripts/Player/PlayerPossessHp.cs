using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessHp : MonoBehaviour
{
    public RectTransform possessHpBar;

    private float maxHp;
    private bool possessing = false;

    private void Start()
    {
        possessHpBar.gameObject.SetActive(possessing);
    }

    public void UpdateBar(float hp)
    {
        if (hp < 0)
            hp = 0;
        else if (hp > maxHp)
            hp = maxHp;

        float Xpos = (1 - (hp / maxHp)) * possessHpBar.sizeDelta.x;
        possessHpBar.anchoredPosition = new Vector2(-Xpos, 0);
    }

    public void SetPossess(bool status)
    {
        possessing = status;
        possessHpBar.gameObject.SetActive(possessing);
    }

    public void SetPossess(float maxHp, float hp, float regeneration)
    {
        this.maxHp = maxHp;
        possessing = true;

        possessHpBar.gameObject.SetActive(possessing);
        UpdateBar(hp);
    }
}
