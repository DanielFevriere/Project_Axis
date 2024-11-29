using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseTarget : AIAction
{
    NavMeshAgent agent;
    EnemyAIBrain brain;
    public float updateTime;
    public float updateTimer;

    public LayerMask whatIsPlayer;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    public float sightRange;
    public float attackRange;

    public bool chasing;

    private void Start()
    {
        //Gets components
        brain = GetComponent<EnemyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        DetermineWeight();

        if (brain.currentState == EnemyAIBrain.AIState.ChasingPlayer)
        {
            updateTimer -= Time.deltaTime;
        }

        //Chases player
        if(chasing)
        {
            //Automatically faces player
            Vector3 targetDirection = brain.target.position - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 150 * Time.deltaTime);

            agent.SetDestination(brain.target.position);
            agent.speed = brain.speed;
            agent.isStopped = false;
        }
        else
        {
            agent.speed = 0;
            agent.isStopped = true;
        }

        if (updateTimer <= 0)
        {
            updateTimer = updateTime;

            //Will think again once the enemy is near the player
            if (brain.currentAction == this)
            {
                if (playerInAttackRange)
                {
                    chasing = false;
                    brain.CompleteAction();
                }
            }
        }
    }

    public override void PerformAction()
    {
        chasing = true;
    }

    public override void DetermineWeight()
    {
        //If the enemy is in attack range the weight is 100
        if (playerInAttackRange)
        {
            weight = 0;
        }
        else if (playerInSightRange)
        {
            weight = 100;
        }
        else if (!playerInSightRange)
        {
            weight = 0;
        }
    }
}
