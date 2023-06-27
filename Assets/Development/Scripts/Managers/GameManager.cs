using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//States the game can be in
public enum GameState { FreeRoam, Battle, Dialogue, Cutscene, Freeze }

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
    public CinemachineVirtualCamera currentCamera;
    [SerializeField] CinemachineVirtualCamera highCamera;
    [SerializeField] CinemachineVirtualCamera lowCamera;
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
        ChangeState(GameState.FreeRoam);
    }

    // Todo: This should tell the game to enter a default state upon launch (for now)
    private void Start()
    {
        ChangeState(GameState.FreeRoam);

        //Goes to dialogue state when dialogue is open
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            if(state != GameState.Cutscene)
            {
                ChangeState(GameState.Dialogue);
            }
        };

        //Goes to freeroam state when dialogue is closed in a regular dialogue, 
        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if(state == GameState.Dialogue)
            {
                ChangeState(GameState.FreeRoam);
            }
        };
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
                    p.GetComponent<NavMeshAgent>().enabled = true;
                }
            }

            if (partyLeader != null)
            {
                worldCamera.GetComponent<DitherObjectsBetweenCamAndPlayer>().target = partyLeader.transform;
                currentCamera.Follow = partyLeader.transform;
            }
           

            /* Anthony: only this should be called here, we only want to keep things that require updates every frame
             * within Update functions, everything else should ideally be event-based instead */
            currentController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            foreach (GameObject p in partyMembers)
            {
                if (p != partyLeader)
                {
                    p.GetComponent<Controller>().enabled = false;
                    p.GetComponent<AIFollow>().enabled = true;
                    p.GetComponent<NavMeshAgent>().enabled = true;
                }
            }

            worldCamera.GetComponent<DitherObjectsBetweenCamAndPlayer>().target = partyLeader.transform;
            currentCamera.Follow = partyLeader.transform;

            /* Anthony: only this should be called here, we only want to keep things that require updates every frame
             * within Update functions, everything else should ideally be event-based instead */
            currentController.inBattle = true;
            currentController.HandleUpdate();

            //Handles the update for the Ability holder as well
            currentController.GetComponentInChildren<AbilityHolder>().HandleUpdate();
        }
        else if (state == GameState.Dialogue)
        {
            foreach (GameObject p in partyMembers)
            {
                p.GetComponent<Controller>().enabled = false;
                p.GetComponent<AIFollow>().enabled = false;
                p.GetComponent<NavMeshAgent>().enabled = false;
            }
            DialogueManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Cutscene)
        {
            currentCamera.Follow = null;
            foreach (GameObject p in partyMembers)
            {
                p.GetComponent<Controller>().enabled = false;
                p.GetComponent<AIFollow>().enabled = false;
                p.GetComponent<NavMeshAgent>().enabled = false;
            }
            DialogueManager.Instance.HandleUpdate();
        }

        // Debug purposes
        //DebugUpdate(); 
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
                Exit_Battle();
                break;
            case GameState.Dialogue:

                break;
            case GameState.Cutscene:

                break;
            case GameState.Freeze:
                currentController.anim.speed = 1f;
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
            case GameState.Dialogue:
                Enter_Dialogue();
                break;
            case GameState.Cutscene:
                Enter_Cutscene();
                break;
            case GameState.Freeze:
                currentController.anim.speed = 0f;
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
        partyLeader.GetComponent<NavMeshAgent>().enabled = false;
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
        AudioManager.Instance.PlayBgm("defiance", 0.0f, 2.0f);

        highCamera.gameObject.SetActive(false);
        lowCamera.gameObject.SetActive(true);
        currentCamera = lowCamera;

        // Todo: Check if partyMembers is empty, if empty then add in party members

        // Todo: Initialize/Set party leader here
        SetPartyLeader();
    }

    void Enter_Battle()
    {
        AudioManager.Instance.PlayBgm("ghoul", 0.0f, 2.0f);

        lowCamera.gameObject.SetActive(false);
        highCamera.gameObject.SetActive(true);
        currentCamera = highCamera;

        foreach (GameObject p in partyMembers)
        {
            partyLeader.GetComponent<Controller>().enabled = false;
            partyLeader.GetComponent<AIFollow>().enabled = false;
        }
    }

    void Enter_Dialogue()
    {

    }

    void Enter_Cutscene()
    {

    }
    #endregion

    #region Exit States

    void Exit_Battle()
    {
        currentController.inBattle = false;
        currentController.running = false;
    }

    #endregion

    #region Camera Controller
    public void ToggleCameraConfiner(bool ConfinderMode)
    {
        CinemachineConfiner confiner = currentCamera.GetComponent<CinemachineConfiner>();
        if (confiner != null)
        {
            confiner.enabled = ConfinderMode;
        }
    }

    public void SetCameraConfiner(BoxCollider NewConfiner)
    {
        CinemachineConfiner confiner = currentCamera.GetComponent<CinemachineConfiner>();
        if (confiner != null)
        {
            if (NewConfiner != null)
            {
                confiner.m_BoundingVolume = NewConfiner;
            }
            else
            {
                // Clears confiner
                confiner.m_BoundingVolume = null;
                confiner.enabled = false;
            }
        }
    }


    #endregion Camera Controller

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
                    nextState = GameState.Dialogue;
                    break;
                case GameState.Dialogue:
                    nextState = GameState.Cutscene;
                    break;
                case GameState.Cutscene:
                    nextState = GameState.FreeRoam;
                    break;
            }

            ChangeState(nextState);
        }
    }
}
