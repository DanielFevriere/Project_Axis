using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : TeamSkill
{
    public float healAmount;
    public override void Activate()
    {
        if (GameManager.Instance.currentTP >= tpCost)
        {
            GameManager.Instance.RemoveTP(tpCost);

            GetComponentInParent<Player>().HealDamage(10);
        }

        Deactivate();
    }
    public override void Deactivate()
    {
        base.Deactivate();
    }
}
