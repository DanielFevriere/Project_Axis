using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAIBrain : MonoBehaviour
{
    public enum AIState { Idle, ChasingPlayer, ChasingEnemy, Attacking}

    public bool triggerThink = false;

    public List<AIAction> freeRoamActions;
    public List<AIAction> combatActions;
    public List<AIAction> availableActions;

    [Header("Data")]
    //Used data
    public LayerMask whatIsPlayer;
    public LayerMask whatIsEnemy;
    public Transform target;

    public bool playerInSightRange;
    public bool enemyInSightRange;
    public bool enemyInAttackRange;
    public float sightRange;
    public float attackRange;

    public float walkSpeed;
    public float runSpeed;

    public AIState currentState;

    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Sequence()
            .AppendInterval(1)
            .AppendCallback(() =>
            {
                //Immediately start thinking after 1 second
                Think();
            });

        //Anytime the games state changes, Think is applied
        GameManager.Instance.OnStateChange += CompleteAction;
        GameManager.Instance.OnStateChange += Think;

        //Anytime the ges, Think is applied
        GameManager.Instance.OnPartyLeaderChange += CompleteAction;
        GameManager.Instance.OnPartyLeaderChange += Think;
    }

    private void Update()
    {
        //DEBUG ONLY
        if(triggerThink)
        {
            Think();
            triggerThink = false;
        }
    }

    public void Think()
    {
        if(GetComponent<Player>().dead)
        {
            return;
        }

        if(!enabled)
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
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
    }


    public void DetermineState()
    {
        //WIP, unsure what to make ally AI individual states at the moment
    }

    //Needs more fleshing out before implementation. Ignore for now
    public void SetGoal()
    {

    }

    //Sets the list of available actions that the AI can do
    public void SetAvailableActions()
    {
        //If you are freeroaming
        if(GameManager.Instance.CurrentState == GameState.FreeRoam)
        {
            //Add all the actions from the freeroaming actions list
            for (int i = 0; i < freeRoamActions.Count; i++)
            {
                availableActions.Add(freeRoamActions[i]);
            }
        }

        //If you are in battle
        if(GameManager.Instance.CurrentState == GameState.Battle)
        {
            //Add all the actions from the combat actions list
            for (int i = 0; i < combatActions.Count; i++)
            {
                availableActions.Add(combatActions[i]);
            }
        }
    }

    //I'll do this next push
    public void RankActions()
    {

    }

    //Decides which action to do based on what the available actions are
    public void ChooseAction()
    {
        //Manual and temporary action choosing until I implement action ranking

        //If in freeroam state
        if(GameManager.Instance.CurrentState == GameState.FreeRoam)
        {
            //Choose random available action
            availableActions[Random.Range(0, availableActions.Count)].PerformAction();
            currentState = AIState.ChasingPlayer;
        }

        //If battle state
        if (GameManager.Instance.CurrentState == GameState.Battle)
        {
            //If the enemy is in attack range,
            if (enemyInAttackRange)
            {
                //Attack
                availableActions[1].PerformAction();
                currentState = AIState.Attacking;
            }
            else if (enemyInSightRange)
            {
                //Chase
                availableActions[0].PerformAction();
                currentState = AIState.ChasingEnemy;
            }
        }
    }

    //Manually called by an AI Action to start the thinking process again.
    public void CompleteAction()
    {
        availableActions.Clear();
        Think();
    }

    private void OnEnable()
    {
        Think();
    }
}
