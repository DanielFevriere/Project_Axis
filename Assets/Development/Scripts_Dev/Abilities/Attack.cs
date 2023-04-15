using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Ability
{
    Controller moveScript;

    public float attackForce;
    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
    }
    public override void Activate()
    {
        StartCoroutine(PushForward());
        moveScript.playerWeapon.Attack();
    }

    IEnumerator PushForward()
    {
        float startTime = Time.time;
        Vector3 movement = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);

        while (Time.time < startTime + abilityTime)
        {
            moveScript.characterController.Move(movement * attackForce * Time.fixedDeltaTime);
            yield return null;
        }
    }
}
