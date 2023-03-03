using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : Ability
{
    Controller moveScript;

    public bool dashing;
    public float dashSpeed;
    public float dashTime;

    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }

    public override void Activate()
    {
        StartCoroutine(DashAbility());
    }

    IEnumerator DashAbility()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            moveScript.characterController.Move(moveScript.movement * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        onCooldown = true;
    }
}
