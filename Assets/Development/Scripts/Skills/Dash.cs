using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : Skill
{
    Controller moveScript;

    public float dashSpeed;
    public Transform atkTransform;


    GameObject owner;
    public ParticleSystem particle;

    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
        if (moveScript)
        {
            owner = moveScript.gameObject;
        }
    }

    public override void Activate()
    {
        GetComponentInParent<Knockback>().OnComplete.AddListener(Deactivate);
        GetComponentInParent<Knockback>().ApplyForwardForce(new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y), dashSpeed, skillTime);
        PlayDashEffect();
        //StartCoroutine(DashAbility());
    }

    public override void Deactivate()
    {
        GetComponentInParent<Knockback>().OnComplete.RemoveListener(Deactivate);
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

        PlayDashEffect();
        
        Deactivate();
    }

    void PlayDashEffect()
    {
        // Checks for valid ownership
        if (!owner)
        {
            return;
        }
        
        Vector3 direction = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);
        
        // Spawns particle based on the unit's facing direction
        ParticleSystem InstantiatedParticle = Instantiate(particle);
        InstantiatedParticle.transform.position = owner.transform.position + Vector3.up * 0.65f + direction * 0.5f;

        InstantiatedParticle.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        
        Destroy(InstantiatedParticle, 1f);
    }
}
