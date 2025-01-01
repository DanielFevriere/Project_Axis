using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseTarget : AIAction
{
    NavMeshAgent agent;
    public float updateTime;
    public float updateTimer;
    public bool chasing;

    private void Start()
    {
        //Gets components
        brain = GetComponent<EnemyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        DetermineWeight();

        updateTimer -= Time.deltaTime;

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
                if (brain.playerInAttackRange)
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
        if (brain.playerInAttackRange)
        {
            weight = 0;
        }
        else if (brain.playerInSightRange)
        {
            weight = 100;
        }
        else if (!brain.playerInSightRange)
        {
            weight = 0;
        }
    }
}
