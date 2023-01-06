using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//States the game can be in
public enum GameState { FreeRoam, Battle, Dialog }

public class GameManager : MonoBehaviour
{
#region Global static reference
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<GameManager>();
                return instance;
            }
        }
    }
    #endregion

    [SerializeField] Controller currentController;
    [SerializeField] Camera worldCamera;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    public GameObject partyLeader;
    public List<GameObject> partyMembers;

    GameState state;
    public GameState CurrentState { get { return state;} }

    // subscribe and unsubscribe events that should happen in other systems when a state change occurs here
    public event Action OnStateChange; 
    public event Action OnPartyLeaderChange;

    [Header("Debug")]
    public bool isDebugging = true;

    private void Awake()
    {
        state = GameState.FreeRoam;
    }

    // Todo: This should tell the game to enter a default state upon launch (for now)
    private void Start()
    {
        ChangeState(GameState.FreeRoam);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            /* Anthony: same as above */
            //Makes sure each non leader party member is following the party leader
            foreach(GameObject p in partyMembers)
            {
                if(p != partyLeader)
                {
                    p.GetComponent<Controller>().enabled = false;
                    p.GetComponent<AIFollow>().enabled = true;
                }
            }

            worldCamera.GetComponent<DitherObjectsBetweenCamAndPlayer>().target = partyLeader.transform;
            virtualCamera.Follow = partyLeader.transform;

            /* Anthony: only this should be called here, we only want to keep things that require updates every frame
             * within Update functions, everything else should ideally be event-based instead */
            currentController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {

        }
        else if (state == GameState.Dialog)
        {

        }

        // Debug purposes
        DebugUpdate();
    }

    public void ChangeState(GameState nextState)
    {
        // If the requested state is the same, do nothing
        //      should probably throw a warning also
        if (nextState == state)
        {
            Debug.LogWarning("SAME STATE change detected! State: " + state.ToString());
            return;
        }

        // Exit current state
        switch (state)
        {
            case GameState.FreeRoam:

                break;
            case GameState.Battle:

                break;
            case GameState.Dialog:

                break;
        }

        // Set next state
        state = nextState;

        // Enter next state
        switch (nextState)
        {
            case GameState.FreeRoam:
                Enter_FreeRoam();
                break;
            case GameState.Battle:
                Enter_Battle();
                break;
            case GameState.Dialog:

                break;
        }

        // Invoke OnStateChange event to broadcast to all listeners
        OnStateChange?.Invoke();
    }

    public void SetPartyLeader()
    {
        //The first party member in the list of party members is the current leader
        partyLeader = partyMembers[0];
        currentController = partyLeader.GetComponent<Controller>();
        partyLeader.GetComponent<Controller>().enabled = true;
        partyLeader.GetComponent<AIFollow>().enabled = false;
    }

    //Swaps control between party members
    public void SwapCharacter()
    {
        //I know this is the lazy way im sorry for my shit code anthony
        partyMembers[0] = partyMembers[1];
        partyMembers[1] = partyMembers[2];
        partyMembers[2] = partyLeader;

        /* Anthony: bruh, instead of swapping like this, keep an INDEX variable that
         * points to the current party leader, then just cycle that variable when ever we need to swap character
         *  index = index + 1
         *  if (index > partyMembers.Length()) { index = 0; }
         *  partyLeader = ...
         */

        SetPartyLeader();
        OnPartyLeaderChange?.Invoke();
    }

    #region Enter States
    void Enter_FreeRoam()
    {
        // Todo: Check if partyMembers is empty, if empty then add in party members

        // Todo: Initialize/Set party leader here
        SetPartyLeader();
    }

    void Enter_Battle()
    {
        foreach(GameObject p in partyMembers)
        {
            partyLeader.GetComponent<Controller>().enabled = false;
            partyLeader.GetComponent<AIFollow>().enabled = false;
        }
    }
    #endregion

    #region Exit States

    #endregion

    void DebugUpdate()
    {
        if (!isDebugging) { return; }

        // State change key (P)
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameState nextState = GameState.FreeRoam;
            switch (state)
            {
                case GameState.FreeRoam:
                    nextState = GameState.Battle;
                    break;
                case GameState.Battle:
                    nextState = GameState.Dialog;
                    break;
                case GameState.Dialog:
                    nextState = GameState.FreeRoam;
                    break;
            }

            ChangeState(nextState);
        }
    }
}
