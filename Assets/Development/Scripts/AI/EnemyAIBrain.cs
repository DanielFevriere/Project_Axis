using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBrain : MonoBehaviour
{
    public enum AIState { Idle, ChasingPlayer, Attacking }

    public bool triggerThink = false;

    public List<AIAction> combatActions;
    public List<AIAction> availableActions;

    [Header("Data")]
    //Target Detection
    public Transform target;
    public string targetTag;
    public LayerMask whatIsPlayer;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    public float sightRange;
    public float attackRange;

    public float speed;

    public AIState currentState;

    void OnEnable()
    {
        DOTween.Sequence()
            .AppendInterval(0.1f)
            .AppendCallback(() =>
            {
                //Immediately start thinking after 1 second
                CompleteAction();
            });
    }

    private void Update()
    {
        //DEBUG ONLY
        if (triggerThink)
        {
            CompleteAction();
            triggerThink = false;
        }
    }

    public void Think()
    {
        if (!enabled)
        {
            return;
        }

        //Collect Data
        CollectData();

        //Determine State
        DetermineState();

        //Set Goal
        SetGoal(); //Ignore for now

        //Set Available Actions
        SetAvailableActions();

        //Rank Available Actions
        RankActions();

        //Chooses Action to perform
        ChooseAction();
    }

    public void CollectData()
    {
        //Grabs a random nearby player for target
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        target = players[Random.Range(0, players.Length)].transform;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }

    public void DetermineState()
    {
        //WIP, unsure what to make enemy AI individual states at the moment
    }

    //Needs more fleshing out before implementation. Ignore for now
    public void SetGoal()
    {

    }

    //Sets the list of available actions that the AI can do
    public void SetAvailableActions()
    {
        //Add all the actions from the combat actions list
        for (int i = 0; i < combatActions.Count; i++)
        {
            availableActions.Add(combatActions[i]);
        }
    }

    //I'll do this next push
    public void RankActions()
    {

    }

    //Decides which action to do based on what the available actions are
    public void ChooseAction()
    {
        //If the enemy is in attack range,
        if (playerInAttackRange)
        {
            //Attack
            availableActions[1].PerformAction();
            currentState = AIState.Attacking;
        }
        else if (playerInSightRange)
        {
            //Chase
            availableActions[0].PerformAction();
            currentState = AIState.ChasingPlayer;
        }
        else if(!playerInSightRange)
        {
            availableActions[2].PerformAction();
            currentState = AIState.Idle;
        }
    }

    //Manually called by an AI Action to start the thinking process again.
    public void CompleteAction()
    {
        availableActions.Clear();
        Think();
    }
}
