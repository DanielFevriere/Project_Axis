using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePartyLeader : AIAction
{
    public GameObject partyLeader;

    public Animator anim;

    NavMeshAgent agent;
    AllyAIBrain brain;

    bool moving = false;
    bool running = false;

    bool performing = false;
    float thinkTime = 0.2f;
    float thinkTimer = 0.2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        partyLeader = GameManager.Instance.partyLeader;
        brain = gameObject.GetComponent<AllyAIBrain>();
    }

    private void Update()
    {
        if(performing)
        {
            thinkTimer -= Time.deltaTime;
        }

        if (thinkTimer <= 0)
        {
            if (brain.enabled)
            {
                thinkTimer = thinkTime;
                performing = false;
                brain.CompleteAction();
            }
        }

        //Sets the direction int in the animator controller
        anim.SetFloat("xAxis", agent.velocity.x);
        anim.SetFloat("yAxis", agent.velocity.z);

        //Sets the direction bool in the animator controller
        anim.SetBool("moving", moving);
        anim.SetBool("running", running);
    }

    //Chases Player
    public override void PerformAction()
    {
        //Does a check to see if there is a party leader
        if (GameManager.Instance.partyLeader == null)
        {
            Debug.LogError(name + " " + "has no party leader assigned! Skipping follow logic.");
            return;
        }

        //Sets Party Leader
        partyLeader = GameManager.Instance.partyLeader;

        //If the leader is running so is the AI
        running = partyLeader.GetComponent<Controller>().running;




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
            agent.speed = brain.runSpeed;
        }
        else
        {
            agent.speed = brain.walkSpeed;
        }

        performing = true;
        agent.SetDestination(partyLeader.transform.position);
    }
}
