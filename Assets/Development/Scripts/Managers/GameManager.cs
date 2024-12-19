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

    public bool isGameplayScene = true;
    public static int MaxPartySize = 3;
    public int partyLeaderIndex = 0;

    [SerializeField] Controller currentController;
    [SerializeField] Camera worldCamera;
    public CinemachineVirtualCamera currentCamera;
    [SerializeField] CinemachineVirtualCamera highCamera;
    [SerializeField] CinemachineVirtualCamera lowCamera;
    [SerializeField] CinemachineVirtualCamera zoomedCamera;
    [SerializeField] CinemachineVirtualCamera bondsMenuCamera;

    public float shakeTimer;
    public float shakeIntensity;


    public GameObject partyLeader;
    public List<GameObject> partyMembers;
    public int maxTP = 30;
    public int currentTP = 0;
    public int veni;

    GameState state;
    public GameState CurrentState { get { return state;} }

    // subscribe and unsubscribe events that should happen in other systems when a state change occurs here
    public event Action OnStateChange; 
    public event Action OnPartyLeaderChange;
    public event Action OnPartyLeaderDeath;
    public event Action OnInteract;
    public event Action OnTPUpdate;

    [Header("Debug")]
    public bool isDebugging = true;

    private void Awake()
    {
        if(isGameplayScene)
        {
            state = GameState.FreeRoam;
            ChangeState(GameState.FreeRoam);
        }
    }

    // Todo: This should tell the game to enter a default state upon launch (for now)
    private void Start()
    {
        //Goes to dialogue state when dialogue is open
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            if (state != GameState.Cutscene)
            {
                ChangeState(GameState.Dialogue);
            }
        };      

        if (isGameplayScene)
        {
            InitializeParty();
        
            SetPartyLeader();

            ChangeState(GameState.FreeRoam);

            //Goes to freeroam state when dialogue is closed in a regular dialogue, 
            DialogueManager.Instance.OnCloseDialogue += () =>
            {
                if(state == GameState.Dialogue)
                {
                    ChangeState(GameState.FreeRoam);
                }
            };
        }
    }

    private void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if (kb.fKey.wasPressedThisFrame)
        {
            OnInteract.Invoke();
        }

        if (isGameplayScene)
        {
            //Reloads scene if dead
            if (partyMembers[0].GetComponent<Player>().dead && partyMembers[1].GetComponent<Player>().dead && partyMembers[2].GetComponent<Player>().dead)
            {
                SceneManager.LoadScene("DevPrototype");
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

            DebugUpdate(); 

        }
        else
        {
            if(state == GameState.Dialogue)
            {
                DialogueManager.Instance.HandleUpdate();
            }

            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;

                if (shakeTimer <= 0f)
                {
                    // Reset the shake effect
                    var noise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    noise.m_AmplitudeGain = 0f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(isGameplayScene)
        {
            if(state == GameState.FreeRoam)
            {
                currentController.HandleFixedUpdate();
            }
            else if(state == GameState.Battle)
            {
                currentController.HandleFixedUpdate();
            }
        }
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
        partyLeader.GetComponent<Controller>().rb.WakeUp();

        //this will make every single skill that is "in use" no longer "in use" (because it shouldn't be in use to begin with if swapping to)
        foreach(Skill s in partyLeader.GetComponentInChildren<SkillHolder>().skillList)
        {
            if(s.inUse == true)
            {
                s.inUse = false;
            }
        }
    }

    /// <summary>
    /// Adds team points
    /// </summary>
    /// <param name="amount"></param>
    public void AddTP(int amount)
    {
        if(currentTP != maxTP)
        {
            currentTP += amount;

            if(currentTP > maxTP)
            {
                currentTP = maxTP;
            }
        }
        OnTPUpdate.Invoke();
    }

    /// <summary>
    /// Removes team points
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveTP(int amount)
    {
        if (currentTP != 0)
        {
            currentTP -= amount;

            if (currentTP < 0)
            {
                currentTP = 0;
            }
        }
        OnTPUpdate.Invoke();
    }

    #endregion

    #region Enter States
    void Enter_FreeRoam()
    {
        AudioManager.Instance.PlayBgm("defiance", 0.0f, 2.0f);

        SetCurrentCamera(lowCamera);

        // Todo: Check if partyMembers is empty, if empty then add in party members

        // Todo: Initialize/Set party leader here
        SetPartyLeader();
    }

    void Enter_Battle()
    {
        //Background music test
        AudioManager.Instance.PlayBgm("ghoul", 0.0f, 2.0f);

        //Swaps the camera to the battle camera
        SetCurrentCamera(highCamera);

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
        //SetCurrentCamera(zoomedCamera);
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

    public void SetCurrentCamera(CinemachineVirtualCamera camera)
    {
        highCamera.gameObject.SetActive(false);
        lowCamera.gameObject.SetActive(false);
        zoomedCamera.gameObject.SetActive(false);
        bondsMenuCamera.gameObject.SetActive(false);

        camera.gameObject.SetActive(true);
        currentCamera = camera;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        var noise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = intensity;
        shakeIntensity = intensity;
        shakeTimer = duration;
    }

    /// <summary>
    /// The only difference is that it has a fixed intensity parameter so it's serializable
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="duration"></param>
    public void CutsceneCameraShake(int duration)
    {
        var noise = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 5;
        shakeIntensity = 5;
        shakeTimer = duration;
    }

    #endregion Camera Controller

    void DebugUpdate()
    {
        if (!isDebugging) { return; }

        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        // State change key (P)
        if (kb.pKey.wasPressedThisFrame)
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

    public void ExitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }

    public CinemachineVirtualCamera HighCamera
    {
        get { return highCamera; }
    }
    public CinemachineVirtualCamera LowCamera
    {
        get { return lowCamera; }
    }
    public CinemachineVirtualCamera ZoomedCamera
    {
        get { return zoomedCamera; }
    }
    public CinemachineVirtualCamera BondsMenuCamera
    {
        get { return bondsMenuCamera; }
    }

}
