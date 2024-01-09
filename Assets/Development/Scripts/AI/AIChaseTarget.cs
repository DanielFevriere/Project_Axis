using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseTarget : AIAction
{
    NavMeshAgent agent;
    EnemyAIBrain brain;

    private void Start()
    {
        //Gets components
        brain = GetComponent<EnemyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void PerformAction()
    {
        //Automatically always facing player
        Vector3 targetDirection = brain.target.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 15 * Time.deltaTime);

        agent.SetDestination(brain.target.position);
        agent.speed = brain.speed;

        agent.isStopped = false;

        DOTween.Sequence()
            .AppendInterval(0.1f)
            .AppendCallback(() =>
            {
                if (brain.enabled)
                {
                    brain.CompleteAction();
                }
            });
    }
}
