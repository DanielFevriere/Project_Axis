using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Attack : Skill
{
    Controller moveScript;
    GameObject owner;

    public ParticleSystem particle;

    public float currentAttackTime;
    public float currentAttackTimer;

    public bool attacking;
    public float attackDamage;
    public float attackForce;
    public float attackDuration;

    //Combo variables
    public bool inCombo = false;

    public float comboTime;
    public float comboTimer;
    public int hitCount = 0;

    public Transform atkTransform;
    public GameObject hitBox;

    void Start()
    {
        currentAttackTimer = currentAttackTime;

        moveScript = GetComponentInParent<Controller>();
        hitBox.GetComponent<DamageOnTouch>().damageDealt = attackDamage;

        if (moveScript)
        {
            owner = moveScript.gameObject;
        }
    }

    private void Update()
    {
        //Combo is no longer on if the combo timer hits 0
        if (comboTimer <= 0)
        {
            DropCombo();
        }

        if (inCombo)
        {
            comboTimer -= Time.deltaTime;
        }

        if (attacking)
        {
            currentAttackTimer -= Time.deltaTime;
        }

        if (currentAttackTimer <= 0)
        {
            currentAttackTimer = currentAttackTime;
            attacking = false;
            hitBox.SetActive(false);
        }

    }

    public override void Activate()
    {
        //If you are still performing the last attack
        if(!attacking)
        {
            inCombo = true;
            
            Hit();
        }
        else
        {
            Deactivate();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    public void Hit()
    {
        if (hitCount != 3)
        {
            //Activates attack animation
            moveScript.anim.SetTrigger("attack");
            PlayAttackEffect();
            hitBox.SetActive(true);
            hitBox.GetComponent<DamageOnTouch>().onCooldown = false;

            hitCount++;
            comboTimer = hitCount == 3 ? skillTime : comboTime;

            //Updates weapon rotation just incase the player faced a different direction for the next attack of the combo
            moveScript.playerWeapon.UpdateWeaponRotation();

            //Adds force
            GetComponentInParent<Knockback>().ApplyForce(atkTransform.position, attackForce, attackDuration);
            attacking = true;
            hitBox.SetActive(true);
        }
        else if(hitCount == 3)
        {
            Deactivate();
        }
    }

    //Drops the combo
    public void DropCombo()
    {
        comboTimer = comboTime;
        hitCount = 0;
        inCombo = false;
    }

    void PlayAttackEffect()
    {
        // Checks for valid ownership
        if (!owner)
        {
            return;
        }

        Vector3 direction = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);

        particle.Play();
    }
}
