using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : Ability
{
    Controller moveScript;

    public float attackForce;

    //Combo variables
    public bool inCombo = false;

    public float comboTime;
    public float comboTimer;
    public int hitCount = 0;

    public Transform atkTransform;

    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }

    private void Update()
    {
        //Combo is no longer on if it hits 0
        if (comboTimer <= 0)
        {
            Deactivate();
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
        comboTimer = comboTime;
        hitCount = 0;
        inCombo = false;
    }

    public void Hit()
    {
        if(hitCount != 3)
        {
            hitCount++;
            comboTimer = hitCount == 3 ? abilityTime : comboTime;
            moveScript.playerWeapon.Attack();

            //Activates attack animation
            moveScript.anim.SetTrigger("attack");

            GetComponentInParent<Knockback>().ApplyKnockback(atkTransform);

            //StartCoroutine(PushForward());
        }
    }

    IEnumerator PushForward()
    {
        float startTime = Time.time;
        Vector3 movement = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);

        while (Time.time < startTime + abilityTime)
        {
            //moveScript.characterController.Move(Vector3.zero);

            moveScript.characterController.Move(movement * attackForce * Time.fixedDeltaTime);
            yield return null;
        }
    }
}
