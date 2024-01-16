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

    public bool chasing;

    private void Start()
    {
        //Gets components
        brain = GetComponent<EnemyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(brain.currentState == EnemyAIBrain.AIState.ChasingPlayer)
        {
            updateTimer -= Time.deltaTime;
        }

        //Automatically always facing player
        Vector3 targetDirection = brain.target.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 150 * Time.deltaTime);

        //Chases player
        if(chasing)
        {
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
            if (brain.currentState == EnemyAIBrain.AIState.ChasingPlayer)
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
}
