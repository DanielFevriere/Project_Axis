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
        visual.SetActive(false);
    }

    public void Hit()
    {
        visual.SetActive(true);
        //Activates attack animation
        moveScript.anim.SetTrigger("attack");
        moveScript.playerWeapon.Attack(); //What i really want is a customized boost strike hitbox, but for now ill use the regular attack hitbox.
    }


    IEnumerator BoostStrikeAbility()
    {
        float startTime = Time.time;
        Vector3 movement = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);

        while (Time.time < startTime + skillTime)
        {
            moveScript.playerWeapon.Attack(); //What i really want is a customized boost strike hitbox, but for now ill use the regular attack hitbox.
            moveScript.characterController.Move(movement * boostStrikeSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        Deactivate();
    }
}
