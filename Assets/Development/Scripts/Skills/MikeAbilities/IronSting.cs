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
        moveScript.playerWeapon.Attack(); //What i really want is a customized hitbox, but for now ill use the regular attack hitbox.
                                          //Adds force
        GetComponentInParent<Knockback>().ApplyKnockback(atkTransform, -ironStingPushBackForce);
    }


    IEnumerator IronStingAbility()
    {
        float startTime = Time.time;

        while (Time.time < startTime + skillTime)
        {
            moveScript.playerWeapon.Attack(); //What i really want is a customized boost strike hitbox, but for now ill use the regular attack hitbox.
            yield return null;
        }

        Deactivate();
    }
}
