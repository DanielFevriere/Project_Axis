using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoostStrike : Skill
{
    Controller moveScript;

    public GameObject visual;
    public float boostStrikeDamage;
    public float boostStrikeSpeed;
    public float boostStrikeDuration;

    public Transform atkTransform;


    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }

    public override void Activate()
    {
        Hit();
        StartCoroutine(BoostStrikeAbility());
    }

    public override void Deactivate()
    {
        base.Deactivate();
        moveScript.anim.SetTrigger("endskill");
        visual.SetActive(false);
    }

    public void Hit()
    {
        visual.SetActive(true);
        //Activates attack animation
        moveScript.anim.SetTrigger("boost");
        GetComponentInParent<Knockback>().ApplyKnockback(atkTransform, boostStrikeSpeed, boostStrikeDuration);

    }


    IEnumerator BoostStrikeAbility()
    {
        float startTime = Time.time;

        while (Time.time < startTime + skillTime)
        {
            yield return null;
        }

        Deactivate();
    }
}
