using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestsTab : GameTab
{
    QuestManager questManager;

    public Quest mainQuest;
    public int currentPage = 1;
    public int pageAmount;
    public GameObject previousPageButton;
    public GameObject nextPageButton;
    public List<Quest> sideQuests;

    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public TMP_Text questRewardText;
    public TMP_Text questConditionsText;

    public TMP_Text mainQuestName;
    public List<TMP_Text> sideQuestDisplayNames;
    public List<GameObject> sideQuestDisplayBgs;

    private void Awake()
    {
        questManager = QuestManager.Instance;
    }

    public override void Refresh()
    {
        //On refresh, clears the quest reference data
        mainQuest = null;
        sideQuests.Clear();

        //Checks to see if there's a main quest
        for (int i = 0; i < questManager.activeQuests.Count; i++)
        {
            if(questManager.activeQuests.Count == 0)
            {
                break;
            }
            //If this active quest is a mainquest, connect the reference
            if(questManager.activeQuests[i].mainQuest)
            {
                mainQuest = questManager.activeQuests[i];
            }
        }

        //Based on the quest page index, figure out the proper formula to display the correct page of quests
        //Utilize the the count of side quest names list, so regardless of if you add more to display per page, itll just work
        for (int i = 0; i < questManager.activeQuests.Count; i++)
        {

            //Adds the quest to the sidequests list if it's not a mainquest
            if(!questManager.activeQuests[i].mainQuest)
            {
                sideQuests.Add(questManager.activeQuests[i]);
            }
        }

        //Name of the mainquest is the name of the first quest in the questmanagers active quests list
        mainQuestName.text = questManager.activeQuests[0].questTitle;

        //Names of the sidequest buttons on the side become the sidequest title
        for (int i = 0; i < sideQuestDisplayNames.Count; i++)
        {
            int targetIndex = sideQuestDisplayNames.Count * (currentPage - 1) + i;

            //specific case where theres not enough quests to fill up a page, break from the loop
            if (targetIndex >= sideQuests.Count)
            {
                sideQuestDisplayNames[i].gameObject.SetActive(false);
                sideQuestDisplayBgs[i].SetActive(false);
                continue;
            }
            //Finds the quest title to show based on the quest page number and size of the list of names needed to display
            Quest targetQuest = sideQuests[sideQuestDisplayNames.Count * (currentPage - 1) + i];

            //If the target quest is not null and not a main quest,
            if(targetQuest != null && !targetQuest.mainQuest)
            {
                //Shows the gameobject
                sideQuestDisplayNames[i].gameObject.SetActive(true);
                sideQuestDisplayBgs[i].SetActive(true);

                //Sets the title
                sideQuestDisplayNames[i].text = targetQuest.questTitle;
            }
        }

        //If sidequests count is 3 and the displayNames count is 4, itll be zero.
        pageAmount = sideQuests.Count / sideQuestDisplayNames.Count;


        if (pageAmount ==  0)
        {
            pageAmount++;
        }
        if((sideQuests.Count % sideQuestDisplayNames.Count) != 0)
        {
            pageAmount++;
        }    

        //If the page number is 1, hide the previous page button
        if(currentPage == 1)
        {
            previousPageButton.SetActive(false);
        }
        //If the page number is greater than 1, show the previous page button
        if(currentPage > 1)
        {
            previousPageButton.SetActive(true);

            //now pageAmount serves as the max amount of pages
            if (currentPage == pageAmount)
            {
                nextPageButton.SetActive(false);
            }
            else
            {
                nextPageButton.SetActive(true);
            }
        }
    }

    public void UpdateQuestDisplay(int questIndex)
    {
        //Searches for the quest title in the sidequests list
        for (int i = 0; i < sideQuests.Count; i++)
        {
            if(sideQuestDisplayNames[questIndex].text == sideQuests[i].questTitle)
            {
                //Displays the quest title, description, rewards, and conditions
                questTitleText.text = sideQuests[i].questTitle;
                questDescriptionText.text = sideQuests[i].questDescription;

                //here's how rewards will be listed out:
                questRewardText.text = sideQuests[i].questReward.RewardsText();

                //Conditions are listed individually
                string conditionText = "";
                for (int j = 0; j < sideQuests[i].conditions.Count; j++)
                {
                    conditionText += sideQuests[i].conditions[j].ConditionName +
                        " " + sideQuests[i].conditions[j].CurrentProgress.ToString() +
                        "/" + sideQuests[i].conditions[j].RequiredProgress.ToString();
                }
                questConditionsText.text = conditionText;
                break;
            }
        }
        
        //uncomment this when u actually bestow rewards
        //questRewardText.text = questManager.activeQuests[questIndex].questReward.rewardName;
    }

    public void NextPage()
    {
        if(currentPage != pageAmount)
        {
            currentPage++;
        }
    }

    public void PreviousPage()
    {
        if(currentPage != 1)
        {
            currentPage--;
        }
    }

}

