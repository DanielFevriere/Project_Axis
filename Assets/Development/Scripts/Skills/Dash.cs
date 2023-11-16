using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : Skill
{
    Controller moveScript;

    public float dashSpeed;

    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }

    public override void Activate()
    {
        StartCoroutine(DashAbility());
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    IEnumerator DashAbility()
    {
        float startTime = Time.time;
        Vector3 movement = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);

        while (Time.time < startTime + skillTime)
        {
            moveScript.characterController.Move(movement * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        Deactivate();
    }
}
