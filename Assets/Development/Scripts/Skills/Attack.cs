using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Attack : Skill
{
    Controller moveScript;

    public float attackForce;

    //Combo variables
    public bool inCombo = false;

    public float comboTime;
    public float comboTimer;
    public int hitCount = 0;

    public Transform atkTransform;
    public GameObject visual;

    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }

    private void Update()
    {
        //Combo is no longer on if it hits 0
        if (comboTimer <= 0)
        {
            //if its already on cooldown, just drop the combo
            if (onCooldown)
            {
                DropCombo();
            }
            //otherwise, activate cooldown and drop the combo
            else
            {
                Deactivate();
            }
        }
        
        if(inCombo)
        {
            comboTimer -= Time.deltaTime;
        }

    }

    public override void Activate()
    {
        inCombo = true;
        Hit();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        visual.SetActive(false);
        comboTimer = comboTime;
        hitCount = 0;
        inCombo = false;
    }

    public void Hit()
    {
        //Activates attack animation
        moveScript.anim.SetTrigger("attack");
        visual.SetActive(true);

        if (hitCount != 3)
        {
            hitCount++;
            comboTimer = hitCount == 3 ? skillTime : comboTime;
            moveScript.playerWeapon.Attack();

            //Adds force
            GetComponentInParent<Knockback>().ApplyKnockback(atkTransform, attackForce);
        }
    }

    //Does the same thing as Deactivate without activating the base class
    public void DropCombo()
    {
        visual.SetActive(false);
        comboTimer = comboTime;
        hitCount = 0;
        inCombo = false;
    }
}
