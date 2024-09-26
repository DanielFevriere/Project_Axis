using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrossBack : Skill
{
    Controller moveScript;

    public GameObject visual;
    public ParticleSystem effect;

    public int crossBackDamage;
    public Transform crossBackStart;
    public Transform crossBackEnd;


    void Start()
    {
        moveScript = GetComponentInParent<Controller>();
        visual.GetComponent<DamageOnTouch>().damageDealt = crossBackDamage;
    }

    public override void Activate()
    {
        Hit();
        StartCoroutine(CrossBackAbility());
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
        ParticleSystem p = Instantiate(effect, visual.transform.position, visual.transform.rotation);

        //Activates attack animation
        //moveScript.anim.SetTrigger("boost");
    }


    IEnumerator CrossBackAbility()
    {
        float startTime = Time.time;
        Vector3 movement = new Vector3(moveScript.facingVector.x, 0, moveScript.facingVector.y);

        while (Time.time < startTime + skillTime)
        {
            yield return null;
        }

        Deactivate();
    }
}
