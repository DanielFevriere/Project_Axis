using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("Character")]
    public CharacterTab CharacterTab;
    [Header("Inventory")]
    public InventoryTab InventoryTab;
    [Header("Skills")]
    public SkillsTab SkillsTab;
    [Header("Quests")]
    public QuestsTab QuestsTab;
    [Header("Bonds")]
    public BondsTab BondsTab;
    [Header("Evolution Atlas")]
    public EvolutionAtlasTab EvolutionAtlasTab;
    [Header("Settings")]
    public SettingsTab SettingsTab;


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

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        gameHudContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class GameMenu
{
    public GameObject gameMenuContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        gameMenuContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class CharacterTab
{
    public GameObject characterTabContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        characterTabContainer.SetActive(isVisible);
    }
}


[System.Serializable]
public class InventoryTab
{
    public GameObject inventoryTabContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        inventoryTabContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class SkillsTab
{
    public GameObject skillsTabContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        skillsTabContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class QuestsTab
{
    public GameObject questsTabContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        questsTabContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class BondsTab
{
    public GameObject bondsTabContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        bondsTabContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class EvolutionAtlasTab
{
    public GameObject evolutionAtlasTabContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        evolutionAtlasTabContainer.SetActive(isVisible);
    }
}

[System.Serializable]
public class SettingsTab
{
    public GameObject settingsTabContainer;

    bool isVisible = true;
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        settingsTabContainer.SetActive(isVisible);
    }
}
