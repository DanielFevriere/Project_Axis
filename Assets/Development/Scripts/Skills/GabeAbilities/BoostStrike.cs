using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoostStrike : Skill
{
    Controller moveScript;

    public GameObject visual;
    public ParticleSystem effect;
    public float boostStrikeDamage;
    public float boostStrikeSpeed;
    public float boostStrikeDuration;

    public Transform atkTransform;


    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
        visual.GetComponent<DamageOnTouch>().damageDealt = boostStrikeDamage;
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
        effect.Stop();
    }

    public void Hit()
    {
        visual.SetActive(true);
        effect.Play();
        //Activates attack animation
        moveScript.anim.SetTrigger("boost");
        GetComponentInParent<Knockback>().ApplyForce(atkTransform.position, boostStrikeSpeed, boostStrikeDuration);
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
