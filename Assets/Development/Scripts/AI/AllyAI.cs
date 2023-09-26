using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAI : MonoBehaviour
{
    public enum AIState { Idle, Chasing, Attacking}

    public List<AIAction> listOfActions;
    public List<AIAction> availableActions;

    [Header("Data")]
    //Used data
    public LayerMask whatIsPlayer;
    public LayerMask whatIsEnemy;
    public bool playerInSightRange;
    public float sightRange;

    public AIState currentState;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Sequence()
            .AppendInterval(1)
            .AppendCallback(() =>
            {
                //Immediately start thinking
                Think();
            });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Think()
    {
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
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    }


    public void DetermineState()
    {
        if(playerInSightRange)
        {
            currentState = AIState.Chasing;
        }
        else
        {
            currentState = AIState.Idle;
        }
    }

    //Needs more fleshing out before implementation. Ignore for now
    public void SetGoal()
    {

    }

    public void SetAvailableActions()
    {
        if(currentState == AIState.Chasing)
        {
            availableActions.Add(listOfActions[0]);
        }
    }

    //I'll do this next push
    public void RankActions()
    {

    }

    public void ChooseAction()
    {
        if(availableActions.Count != 0)
        {
            availableActions[0].PerformAction();
        }

        DOTween.Sequence()
            .AppendInterval(0.1f)
            .AppendCallback(() =>
            {
                CompleteAction();
            });
    }

    public void CompleteAction()
    {
        availableActions.Clear();

        Think();
    }
}
