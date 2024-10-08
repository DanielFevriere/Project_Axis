using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    QuestManager questManager;

    public GameObject gameHudContainer;
    public GameObject battleHud;
    bool isVisible = false;
    bool isBattleHudVisible = false;
    private void Awake()
    {
        questManager = QuestManager.Instance;
        battleHud.SetActive(false);
        GameManager.Instance.OnStateChange += ToggleBattleHudVisibility;
    }

    private void Update()
    {
        string titleText = "";
        string conditionText = "";

        if(questManager.activeQuests.Count != 0)
        {
            if(questManager.highlightedQuest != null)
            {
                for (int i = 0; i < questManager.highlightedQuest.conditions.Count; i++)
                {
                    conditionText = questManager.highlightedQuest.conditions[i].ConditionName +
                        " " + questManager.highlightedQuest.conditions[i].CurrentProgress.ToString() +
                        "/" + questManager.highlightedQuest.conditions[i].RequiredProgress.ToString();
                }
                titleText = questManager.highlightedQuest.questTitle;
            }
        }

        //Sets the HUD text to the active target quest
        questTitleText_HUD.text = titleText;
        questConditionText_HUD.text = conditionText;
    }
    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        gameHudContainer.SetActive(isVisible);
    }

    /// <summary>
    /// Toggles the visibility of the battle ui
    /// </summary>
    public void ToggleBattleHudVisibility()
    {
        battleHud.SetActive(GameManager.Instance.CurrentState == GameState.Battle ? true : false);
    }

    public TMP_Text questTitleText_HUD;
    public TMP_Text questConditionText_HUD;
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