using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseEnemy : AIAction
{
    public GameObject target;

    public Animator anim;

    NavMeshAgent agent;
    AllyAIBrain brain;
    bool moving = false;
    bool running = false;

    private void Awake()
    {
        brain = gameObject.GetComponent<AllyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }

    //Chases Player
    public override void PerformAction()
    {
        running = true;

        //Grabs a random nearby player for target
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = enemies[Random.Range(0, enemies.Length)];

        if (agent.velocity.x == 0 && agent.velocity.y == 0)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }

        if (running)
        {
            agent.speed = brain.walkSpeed;
        }
        else
        {
            agent.speed = brain.walkSpeed;
        }



        //Sets the direction int in the animator controller
        anim.SetFloat("xAxis", agent.velocity.x);
        anim.SetFloat("yAxis", agent.velocity.y);

        anim.SetBool("moving", moving);
        anim.SetBool("running", running);

        agent.SetDestination(target.transform.position);

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

