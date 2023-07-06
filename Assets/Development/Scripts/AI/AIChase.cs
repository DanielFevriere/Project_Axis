using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChase : AIAction
{
    public NavMeshAgent agent;

    public GameObject partyLeader;
    public float walkSpeed;
    public float runSpeed;

    public Animator anim;

    bool moving = false;
    bool running = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        partyLeader = GameManager.Instance.partyLeader;
    }

    private void Update()
    {
        if (GameManager.Instance.partyLeader == null)
        {
            Debug.LogError(name + " " + "has no party leader assigned! Skipping follow logic.");
            return;
        }
        if(agent.velocity.x == 0 && agent.velocity.y == 0)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }

        if(running)
        {
            agent.speed = runSpeed;
        }
        else
        {
            agent.speed = walkSpeed;
        }

        //If the leader is running so is the AI
        running = partyLeader.GetComponent<Controller>().running;

        //Sets the direction int in the animator controller
        anim.SetFloat("xAxis", agent.velocity.x);
        anim.SetFloat("yAxis", agent.velocity.y);

        anim.SetBool("moving", moving);
        anim.SetBool("running", running);
    }

    //Chases Player
    public override void PerformAction()
    {
        partyLeader = GameManager.Instance.partyLeader;

        agent.SetDestination(partyLeader.transform.position);
    }
}
