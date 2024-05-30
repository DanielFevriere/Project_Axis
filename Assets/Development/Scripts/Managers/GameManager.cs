using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    public static int MaxPartySize = 3;
    public int partyLeaderIndex = 0;

    [SerializeField] Controller currentController;
    [SerializeField] Camera worldCamera;
    public CinemachineVirtualCamera currentCamera;
    [SerializeField] CinemachineVirtualCamera highCamera;
    [SerializeField] CinemachineVirtualCamera lowCamera;
    public GameObject partyLeader;
    public List<GameObject> partyMembers;
    public int veni;

    GameState state;
    public GameState CurrentState { get { return state;} }

    // subscribe and unsubscribe events that should happen in other systems when a state change occurs here
    public event Action OnStateChange; 
    public event Action OnPartyLeaderChange;
    public event Action OnPartyLeaderDeath;
    public event Action OnInteract;

    [Header("Debug")]
    public bool isDebugging = true;

    private void Awake()
    {
        //partyLeader = partyMembers[0]; //no this is fucking terrible
        state = GameState.FreeRoam;
        ChangeState(GameState.FreeRoam);
    }

    // Todo: This should tell the game to enter a default state upon launch (for now)
    private void Start()
    {
        InitializeParty();
        
        SetPartyLeader();

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
        if (partyMembers[0].GetComponent<Player>().dead && partyMembers[1].GetComponent<Player>().dead && partyMembers[2].GetComponent<Player>().dead)
        {
            SceneManager.LoadScene("DevPrototype");
        }

        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if(kb.fKey.wasPressedThisFrame)
        {
            OnInteract.Invoke();
        }

        if (state == GameState.FreeRoam)
        {
            /* Anthony: same as above */
            //Makes sure each non leader party member is following the party leader
            foreach(GameObject p in partyMembers)
            {
                if(p != partyLeader)
                {
                    p.GetComponent<Controller>().enabled = false;
                    p.GetComponent<NavMeshAgent>().enabled = true;

                    foreach(AIAction a in p.GetComponent<AllyAIBrain>().freeRoamActions)
                    {
                        a.enabled = true;
                    }
                    foreach (AIAction a in p.GetComponent<AllyAIBrain>().combatActions)
                    {
                        a.enabled = true;
                    }

                    p.GetComponent<AllyAIBrain>().enabled = true;
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
                    //Turns off the aimer graphic for all party members
                    p.GetComponent<Controller>().aimerGraphic.SetActive(false);

                    p.GetComponent<Controller>().enabled = false;
                    p.GetComponent<NavMeshAgent>().enabled = true;

                    p.GetComponent<AllyAIBrain>().enabled = true;
                    foreach (AIAction a in p.GetComponent<AllyAIBrain>().freeRoamActions)
                    {
                        a.enabled = true;
                    }
                    foreach (AIAction a in p.GetComponent<AllyAIBrain>().combatActions)
                    {
                        a.enabled = true;
                    }
                }
                else
                {
                    //Turns on aimer graphic for the party leader
                    p.GetComponent<Controller>().aimerGraphic.SetActive(true);
                }
            }

            worldCamera.GetComponent<DitherObjectsBetweenCamAndPlayer>().target = partyLeader.transform;
            currentCamera.Follow = partyLeader.transform;

            /* Anthony: only this should be called here, we only want to keep things that require updates every frame
             * within Update functions, everything else should ideally be event-based instead */
            currentController.inBattle = true;
            currentController.HandleUpdate();

            //Handles the update for the Ability holder as well
            currentController.GetComponentInChildren<SkillHolder>().HandleUpdate();
        }
        else if (state == GameState.Dialogue)
        {
            foreach (GameObject p in partyMembers)
            {
                p.GetComponent<Controller>().enabled = false;
                p.GetComponent<NavMeshAgent>().enabled = false;

                foreach (AIAction a in p.GetComponent<AllyAIBrain>().freeRoamActions)
                {
                    a.enabled = false;
                }
                foreach (AIAction a in p.GetComponent<AllyAIBrain>().combatActions)
                {
                    a.enabled = false;
                }

                p.GetComponent<AllyAIBrain>().enabled = false;
            }
            DialogueManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Cutscene)
        {
            currentCamera.Follow = null;
            foreach (GameObject p in partyMembers)
            {
                p.GetComponent<Controller>().enabled = false;
                p.GetComponent<NavMeshAgent>().enabled = false;

                foreach (AIAction a in p.GetComponent<AllyAIBrain>().freeRoamActions)
                {
                    a.enabled = false;
                }
                foreach (AIAction a in p.GetComponent<AllyAIBrain>().combatActions)
                {
                    a.enabled = false;
                }

                p.GetComponent<AllyAIBrain>().enabled = false;
            }
            DialogueManager.Instance.HandleUpdate();
        }

        // Debug/Test purposes

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

    #region Party Management

    // Called in Player awake, adds a new character to party list and initialize all UIs
    public void TryAddCharacterToPlayerParty(GameObject Character)
    {
        if (partyMembers.Count < MaxPartySize &&
            !partyMembers.Contains(Character))
        {
            partyMembers.Add(Character);
            
            // Notify UI
            UiManager.Instance.Hud.SetUpCombatStatusPanelForCharacter(Character.GetComponent<Stats>(), partyMembers.Count - 1);
        }
    }

    void InitializeParty()
    {
        if (partyMembers.Count > 0)
        {
            for (int i = 0; i < partyMembers.Count; i++)
            {
                // Notify UI
                UiManager.Instance.Hud.SetUpCombatStatusPanelForCharacter(partyMembers[i].GetComponent<Stats>(), i);
            }
        }
    }
    
    public void SetPartyLeader()
    {
        if (partyMembers.Count == 0)
        {
            return;
        }



        //The first party member in the list of party members is the current leader
        partyLeader = partyMembers[partyLeaderIndex];
        currentController = partyLeader.GetComponent<Controller>();
        partyLeader.GetComponent<Controller>().enabled = true;
        partyLeader.GetComponent<NavMeshAgent>().enabled = false;


        foreach (AIAction a in partyLeader.GetComponent<AllyAIBrain>().freeRoamActions)
        {
            a.enabled = false;
        }
        foreach (AIAction a in partyLeader.GetComponent<AllyAIBrain>().combatActions)
        {
            a.enabled = false;
        }

        partyLeader.GetComponent<AllyAIBrain>().enabled = false;
    }

    //Swaps control between party members
    public void SwapCharacter()
    {
        //Swaps party leader

        if (partyLeaderIndex == partyMembers.Count - 1)
        {
            partyLeaderIndex = 0;
        }
        else
        {
            partyLeaderIndex = partyLeaderIndex + 1;
        }

        //if that party leader is dead, swap to the next one (Recursive function)
        if (partyMembers[partyLeaderIndex].GetComponent<Player>().dead)
        {
            SwapCharacter();
        }

        SetPartyLeader();
        OnPartyLeaderChange?.Invoke();
    }

    #endregion

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
        //Background music test
        AudioManager.Instance.PlayBgm("ghoul", 0.0f, 2.0f);

        //Swaps the camera to the battle camera
        lowCamera.gameObject.SetActive(false);
        highCamera.gameObject.SetActive(true);
        currentCamera = highCamera;

        //Teleports each party member to the party leader upon entering the battle state
        foreach (GameObject p in partyMembers)
        {
            if(p != partyLeader)
            {
                p.transform.position = partyLeader.transform.position;
            }
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
        foreach (GameObject p in partyMembers)
        {
            //Turns off the aimer graphic for all party members
            p.GetComponent<Controller>().aimerGraphic.SetActive(false);
        }

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
