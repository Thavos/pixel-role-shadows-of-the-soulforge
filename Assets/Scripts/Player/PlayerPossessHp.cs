using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessHp : MonoBehaviour
{
    public RectTransform possessHpBar;

    private int maxHp, hp;
    private bool possessing = false;

    private void Start()
    {
        possessHpBar.gameObject.SetActive(possessing);
    }

    public void UpdateBar(int hp)
    {
        if (hp < 0)
            hp = 0;
        else if (hp > maxHp)
            hp = maxHp;

        float Xpos = (1 - (hp / maxHp)) * -possessHpBar.sizeDelta.x;
        possessHpBar.localPosition = new Vector3(Xpos, 0, 0);
    }

    public void SetPossess(bool status)
    {
        possessing = status;
        possessHpBar.gameObject.SetActive(possessing);
    }

    public void SetPossess(int maxHp, int hp, int regeneration)
    {
        this.maxHp = maxHp;
        possessing = true;

        possessHpBar.gameObject.SetActive(possessing);
        UpdateBar(hp);
    }
}
