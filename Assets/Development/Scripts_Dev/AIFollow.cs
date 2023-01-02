using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFollow : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer;

    //Patrolling

    //States
    public float sightRange;
    public bool playerInSightRange;

    private void Awake()
    {
        player = GameObject.Find("Character").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        //Chases player if theyre in range
        if (playerInSightRange) ChasePlayer();
    }

    //Chases Player
    void ChasePlayer()
    {
        //Vector3 distanceToPlayer = transform.position - player.position;
        agent.SetDestination(player.position);
    }
}
