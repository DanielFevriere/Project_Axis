using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoostStrike : Skill
{
    Controller moveScript;

    public float boostStrikeSpeed;

    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }

    public override void Activate()
    {
        StartCoroutine(BoostStrikeAbility());
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
    IEnumerator BoostStrikeAbility()
    {
        float startTime = Time.fixedTime;
        Vector3 movement = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);

        while (Time.fixedTime < startTime + skillTime)
        {
            moveScript.characterController.Move(movement * boostStrikeSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        Deactivate();
    }
}
