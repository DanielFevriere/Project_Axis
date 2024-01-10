using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region Global static reference
    private static UiManager instance;
    public static UiManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<UiManager>();
                return instance;
            }
        }
    }
    #endregion

    #region Sub components
    [Header("Debugger")]
    public UiDebug Debug;
    [Header("Game HUD")]
    public GameHUD Hud;
    [Header("Game Menu")]
    public GameMenu Menu;

    [Header("Game Menu Tabs")]
    public List<GameTab> gameTabs;

    [Header("Character")]
    public GameTab CharacterTab;
    [Header("Inventory")]
    public GameTab InventoryTab;
    [Header("Skills")]
    public GameTab SkillsTab;
    [Header("Quests")]
    public GameTab QuestsTab;
    [Header("Bonds")]
    public GameTab BondsTab;
    [Header("Evolution Atlas")]
    public GameTab EvolutionAtlasTab;
    [Header("Settings")]
    public GameTab SettingsTab;


    public Animator BlackScreenAnimator;

    #endregion

    private void Start()
    {
        GameManager.Instance.OnStateChange += DEBUG_SetCurrentState;
        GameManager.Instance.OnPartyLeaderChange += DEBUG_SetCurrentPartyLeader;

        // first run
        DEBUG_SetCurrentState();
        DEBUG_SetCurrentPartyLeader();

        Debug.ToggleVisibility();

        Hud.ToggleVisibility();

        gameTabs.Add(CharacterTab);
        gameTabs.Add(InventoryTab);
        gameTabs.Add(SkillsTab);
        gameTabs.Add(QuestsTab);
        gameTabs.Add(BondsTab);
        gameTabs.Add(EvolutionAtlasTab);
        gameTabs.Add(SettingsTab);
    }

    public void DEBUG_SetCurrentState()
    {
        Debug.gameStateText.text = "Game State: " + GameManager.Instance.CurrentState.ToString();
    }

    public void DEBUG_SetCurrentPartyLeader()
    {
        GameManager.Instance.SetPartyLeader();
        if (GameManager.Instance.partyLeader != null)
        {
            Debug.partyLeaderText.text = "Party Leader: " + GameManager.Instance.partyLeader.name;
            Debug.partyLeaderText.color = Color.white;
            return;
        }

        Debug.partyLeaderText.text = "Party Leader NOT FOUND!";
        Debug.partyLeaderText.color = Color.red;
    }
    public void PlayBlackScreenEffect()
    {
        BlackScreenAnimator.SetTrigger("Fade");
    }

    public void SwitchToTab(string tabName)
    {
        foreach(GameTab g in gameTabs)
        {
            if(g.tabName != tabName)
            {
                g.isVisible = true;
            }
            else
            {
                g.isVisible = false;
            }
            g.ToggleVisibility();
        }
    }
}

[System.Serializable]
public class UiDebug
{
    public GameObject debugUiContainer;
    public TMPro.TMP_Text gameStateText;
    public TMPro.TMP_Text partyLeaderText;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        debugUiContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class GameHUD
{
    public GameObject gameHudContainer;

    bool isVisible = false;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        gameHudContainer.SetActive(isVisible);
    }

    public List<CharacterCombatStatusPanel> CombatStatusPanels;

    public void SetUpCombatStatusPanelForCharacter(Stats OwnerStats, int index = 0)
    {
        if (CombatStatusPanels.Count == 0)
        {
            Debug.LogError("CombatStatusPanels unassigned in UI!!");
            return;
        }

        CombatStatusPanels[index].SetUpNewOwner(OwnerStats);
    }
}

[System.Serializable]
public class GameMenu
{
    public GameObject gameMenuContainer;

    bool isVisible = false;
    
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        gameMenuContainer.SetActive(isVisible);
    }
}