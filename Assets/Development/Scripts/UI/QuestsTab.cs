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

    public TMP_Text questTitleText_HUD;
    public TMP_Text questConditionText_HUD;

    public TMP_Text mainQuestName;
    public List<TMP_Text> sideQuestNames;


    private void Awake()
    {
        questManager = QuestManager.Instance;
    }



    public override void Refresh()
    {
        if(QuestManager.Instance.activeQuests.Count == 0)
        {
            mainQuest = questManager.activeQuests[0];
            return;
        }

        sideQuests.Clear();

        for (int i = 1; i < questManager.activeQuests.Count; i++)
        {
            UpdateQuestDisplay(i);
            sideQuests.Add(questManager.activeQuests[i]);
        }

        //Name of the mainquest is the name of the first quest in the questmanagers active quests list
        mainQuestName.text = questManager.activeQuests[0].questTitle;

        //Names of the sidequest buttons on the side become the sidequest title
        for (int i = 0; i < sideQuestNames.Count; i++)
        {
            sideQuestNames[i].text = sideQuests[i].questTitle;
        }
    }

    public void UpdateQuestDisplay(int questIndex)
    {
        questTitleText.text = questManager.activeQuests[questIndex].questTitle;
        questDescriptionText.text = questManager.activeQuests[questIndex].questDescription;
        //uncomment this when u actually bestow rewards
        //questRewardText.text = questManager.activeQuests[questIndex].questReward.rewardName;
    }

}

