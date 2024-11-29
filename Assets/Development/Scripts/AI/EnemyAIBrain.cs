using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBrain : MonoBehaviour
{
    public enum AIState { Idle, ChasingPlayer, Attacking }

    public bool triggerThink = false;

    public AIAction currentAction;
    public List<AIAction> combatActions;
    public List<AIAction> availableActions;

    [Header("Data")]
    //Target Detection
    public Transform target;
    public string targetTag;

    public float speed;

    public float thinkTime;
    public float thinkTimer;

    public AIState currentState;

    void OnEnable()
    {
        triggerThink = true;
    }

    private void Update()
    {
        CollectData();

        //DEBUG ONLY
        if (triggerThink)
        {
            CompleteAction();
            triggerThink = false;
        }
    }

    public void Think()
    {
        //Enemy only thinks when it's not stunned
        if(!GetComponent<Enemy>().stunned)
        {
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
    }

    public void CollectData()
    {
        //Grabs a random nearby player for target
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> playerList = new List<GameObject>();

        //Makes it into a list for easy removal
        foreach (GameObject p in players)
        {
            playerList.Add(p);
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<Player>().dead)
            {
                playerList.Remove(players[i]);
            }
        }

        players = playerList.ToArray();
        target = FindClosestObject(players).transform;
    }

    public void DetermineState()
    {
        //WIP, unsure what to make enemy AI individual states at the moment
        //I think some basic states should be Attacking, Defending, 
    }

    //Needs more fleshing out before implementation. Ignore for now
    public void SetGoal()
    {
        //Goals for enemies could be simple
        //Goals are priorities
        //Simple goals are best
        //The goals guides how much of a priority an action should be 
        //Recommendation: Kill Player, Survive, Stall, Assist
        //The state will determine the goal
    }

    //Sets the list of available actions that the AI can do
    public void SetAvailableActions()
    {
        foreach(AIAction a in combatActions)
        {
            if(a.weight != 0)
            {
                availableActions.Add(a);
            }
        }
    }

    //I'll do this next push
    public void RankActions()
    {

    }

    //Decides which action to do based on weight of the available actions
    public void ChooseAction()
    {
        //Add up the total weight of all actions 
        float totalWeight = 0;

        foreach(AIAction action in availableActions)
        {
            totalWeight += action.weight;
        }

        //Generate a random value betwen 0 and the total weight
        float randomValue = Random.Range(0, totalWeight);

        // Select action based on weight
        float cumulativeWeight = 0;
        foreach (AIAction action in availableActions)
        {
            cumulativeWeight += action.weight;
            if (randomValue < cumulativeWeight)
            {
                currentAction = action;
                action.PerformAction();
            }
        }

        // Fallback (shouldn't happen if weights are set properly)
        Debug.LogWarning("No action selected; check weights.");


        /*

        //uncomment if the previous logic stops working

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
        */
        
    }

    //Manually called by an AI Action to start the thinking process again.
    public void CompleteAction()
    {
        availableActions.Clear();
        Think();
    }

    GameObject FindClosestObject(GameObject[] objectsArray)
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        // Loop through each object in the array
        foreach (GameObject obj in objectsArray)
        {
            // Calculate the distance between the targetTransform and the current object
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            // Check if the current object is closer than the previously found closest object
            if (distance < closestDistance)
            {
                closestObject = obj;
                closestDistance = distance;
            }
        }

        return closestObject;
    }
}
