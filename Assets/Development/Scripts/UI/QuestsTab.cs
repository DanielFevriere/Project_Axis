using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestsTab : GameTab
{
    QuestManager questManager;

    public Quest mainQuest;

    public List<Quest> sideQuests;

    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public TMP_Text questRewardText;

    public List<string> sideQuestNames;

    public void Refresh()
    {
        //Names of the sidequest buttons on the side become the sidequest title
        for (int i = 0; i < sideQuestNames.Count; i++)
        {
            sideQuestNames[i] = sideQuests[i].questTitle;
        }
    }

    void UpdateQuestDisplay(int questIndex)
    {
        questTitleText.text = questManager.activeQuests[questIndex].questTitle;
        questDescriptionText.text = questManager.activeQuests[questIndex].questDescription;
        questRewardText.text = questManager.activeQuests[questIndex].questReward.rewardName;
    }

}

