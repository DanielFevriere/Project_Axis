using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackPlayer : AIAction
{
    NavMeshAgent agent;

    public GameObject attackBox;
    public GameObject warningBox;
    public ParticleSystem particle;



    public bool onCooldown = false;
    public bool windingUpAttack;
    public bool attacking;

    public float attackWindupTime;
    public float attackWindupTimer;

    public float attackTime;
    public float attackTimer;

    public float attackCoolDownTime;
    public float attackCoolDownTimer;

    private void Start()
    {
        brain = GetComponent<EnemyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        DetermineWeight();

        // Get the current rotation of the object
        Quaternion currentRotation = transform.rotation;

        // Lock the rotation on the X and Z axes
        currentRotation.eulerAngles = new Vector3(0, currentRotation.eulerAngles.y, 0);

        // Apply the new rotation
        transform.rotation = currentRotation;
        


        if (onCooldown)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        if(attackCoolDownTimer <= 0)
        {
            attackCoolDownTimer = attackCoolDownTime;
            onCooldown = false;
        }

        if(windingUpAttack)
        {
            attackWindupTimer -= Time.deltaTime;
        }    

        if(attackWindupTimer <= 0)
        {
            attackWindupTimer = attackWindupTime;
            windingUpAttack = false;

            Attack();
        }

        if(attacking)
        {
            attackTimer -= Time.deltaTime;
        }

        if(attackTimer <= 0)
        {
            attackBox.SetActive(false);
            attackTimer = attackTime;
            attacking = false;
            onCooldown = true;
            brain.CompleteAction();
        }
    }

    public override void PerformAction()
    {
        WindupAttack();
    }

    public override void DetermineWeight()
    {
        //If the enemy is in attack range the weight is 100
        if (brain.playerInAttackRange)
        {
            weight = 100;
        }
        else if (brain.playerInSightRange)
        {
            weight = 0;
        }
        else if (!brain.playerInSightRange)
        {
            weight = 0;
        }

    }

    public void WindupAttack()
    {
        //Automatically faces player
        Vector3 targetDirection = brain.target.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 150 * Time.deltaTime);

        windingUpAttack = true;
        warningBox.SetActive(true);
    }

    public void Attack()
    {
        attacking = true;
        warningBox.SetActive(false);
        attackBox.SetActive(true);
        particle.Play();
    }
}
