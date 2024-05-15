using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IronSting : Skill
{
    Controller moveScript;

    public GameObject visual;
    public float ironStingDamage;
    public float ironStingPushBackForce;
    public float ironStingPushBackDuration;
    public Transform atkTransform;


    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }

    public override void Activate()
    {
        Hit();
        StartCoroutine(IronStingAbility());
    }

    public override void Deactivate()
    {
        base.Deactivate();
        //moveScript.anim.SetTrigger("endskill");
        visual.SetActive(false);
    }

    public void Hit()
    {
        visual.SetActive(true);
        //Activates attack animation
        //moveScript.anim.SetTrigger("boost");


        //Adds force
        GetComponentInParent<Knockback>().ApplyKnockback(atkTransform, -ironStingPushBackForce, ironStingPushBackDuration);
    }


    IEnumerator IronStingAbility()
    {
        float startTime = Time.time;

        while (Time.time < startTime + skillTime)
        {
            yield return null;
        }

        Deactivate();
    }
}
