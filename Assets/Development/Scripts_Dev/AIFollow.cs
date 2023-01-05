using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFollow : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer;

    //States
    public float sightRange;
    public bool playerInSightRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GameManager.Instance.partyLeader == null)
        {
            Debug.LogError(name + " " + "has no party leader assigned! Skipping follow logic.");
            return;
        }

        player = GameManager.Instance.partyLeader.transform;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        //Chases player if theyre in range
        if (playerInSightRange) ChasePlayer();
    }

    //Chases Player
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
}
