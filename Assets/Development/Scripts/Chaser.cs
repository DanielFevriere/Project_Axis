using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    public enum ChaserState { Chasing, Attacking, Detecting }

    //Current State
    public ChaserState state;
    
    //Navmesh Agent
    public NavMeshAgent agent;

    //Target Detection
    public Transform target;
    public string targetTag;
    public LayerMask targetLayer;

    //Range Detection
    public float sightRange;
    public bool targetInChaseRange = false;
    public float attackRange;
    public bool targetInAttackRange = false;

    //Attacking Vars
    public bool inAttack = false;
    public bool onCooldown = false;
    public GameObject attackBox;
    public GameObject warningBox;
    public float attackWindupTime;
    public float attackTime;
    public float attackCoolDown;


    // Start is called before the first frame update
    void Start()
    {
        //Gets components
        agent = GetComponent<NavMeshAgent>();

        //Sets state to Detecting at the start
        state = ChaserState.Detecting;
        DetectTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if target is in the chase or attacking range
        targetInChaseRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        //Automatically always facing player
        Vector3 targetDirection = target.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 15 * Time.deltaTime);

        //State Changes based on if target is in chasing or attack range
        if (targetInChaseRange)
        {
            if(!targetInAttackRange)
            {
                state = ChaserState.Chasing;
            }
            else
            {
                state = ChaserState.Attacking;
                agent.isStopped = true;
            }
        }
        else
        {
            state = ChaserState.Detecting;
            agent.isStopped = true;
        }

        //Action Depending on state
        switch (state)
        {
            //Checks to see if target is in range
            case ChaserState.Detecting:
                break;

            //Chases player if theyre in range
            case ChaserState.Chasing:
                ChaseTarget();
                break;

            //Attacks Target if theyre in range
            case ChaserState.Attacking:
                if(!onCooldown && !inAttack)
                {
                    AttackTarget();
                }
                break;
        }
    }

    //Detects Target
    void DetectTarget()
    {
        //Grabs a random nearby player for target
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        target = players[Random.Range(0, players.Length)].transform;
    }

    //Chases Target
    void ChaseTarget()
    {

        agent.SetDestination(target.position);
        agent.isStopped = false;
    }

    //Attacks Target
    void AttackTarget()
    {
        inAttack = true;

        DOTween.Sequence()
        //Turns on warning box
        .AppendCallback(() =>
        {
            warningBox.SetActive(true);
        })
        .AppendInterval(attackWindupTime)
        //Turns on Attack box, turns off warning box
        .AppendCallback(() =>
        {
            warningBox.SetActive(false);
            attackBox.SetActive(true);
            onCooldown = true;
        })
        //Turns off Attack box, goes on cooldown
        .AppendInterval(attackTime)
        .AppendCallback(() =>
        {
            inAttack = false;
            attackBox.SetActive(false);
            onCooldown = true;
        })
        .AppendInterval(attackCoolDown)
        //Goes off cooldown
        .AppendCallback(() =>
        {
            onCooldown = false;
        }); 
    }
}
