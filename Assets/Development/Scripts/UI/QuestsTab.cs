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
    public GameObject highlightQuestButton;
    public GameObject previousPageButton;
    public GameObject nextPageButton;
    public List<Quest> sideQuests;

    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public TMP_Text questRewardText;
    public TMP_Text questConditionsText;

    public TMP_Text mainQuestDisplayName;
    public GameObject mainQuestDisplayBg;
    public List<TMP_Text> sideQuestDisplayNames;
    public List<GameObject> sideQuestDisplayBgs;

    private void Awake()
    {
        questManager = QuestManager.Instance;
        ClearQuestDisplay();
    }

    public override void Refresh()
    {
        //On refresh, clears the quest reference data
        mainQuest = null;
        sideQuests.Clear();

        if (questManager.activeQuests.Count == 0)
        {
            ClearQuestDisplay();
            highlightQuestButton.SetActive(false);
        }
        else
        {
            highlightQuestButton.SetActive(true);
        }

        //Checks to see if there's a main quest
        for (int i = 0; i < questManager.activeQuests.Count; i++)
        {
            if(questManager.activeQuests.Count == 0)
            {
                ClearQuestDisplay();
                break;
            }
            //If this active quest is a mainquest, connect the reference
            if(questManager.activeQuests[i].mainQuest)
            {
                mainQuest = questManager.activeQuests[i];
            }
        }

        //If theres no mainquest, Disable the visual
        if (mainQuest == null)
        {
            mainQuestDisplayName.gameObject.SetActive(false);
            mainQuestDisplayBg.SetActive(false);
        }
        else
        {
            //Otherwise enable
            mainQuestDisplayName.gameObject.SetActive(true);
            mainQuestDisplayBg.SetActive(true);

            //Sets the displayname of the main quest
            mainQuestDisplayName.text = mainQuest.questTitle;
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

        //Sets the page amount
        pageAmount = sideQuests.Count / sideQuestDisplayNames.Count;

        if (pageAmount ==  0)
        {
            pageAmount++;
        }
        else if((sideQuests.Count % sideQuestDisplayNames.Count) != 0)
        {
            pageAmount++;
        }    

        //If the page number is 1, hide the previous page button
        if(currentPage == 1)
        {
            previousPageButton.SetActive(false);
        }

        //If theres only one page, hide the next page button
        if(pageAmount == 1)
        {
            nextPageButton.SetActive(false);
        }
        else
        {
            nextPageButton.SetActive(true);
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

        //Highlightedquest null check
        if(questManager.highlightedQuest != null)
        {
            //If the displayedQuest is the highlighted quest
            if (questTitleText.text == questManager.highlightedQuest.questTitle)
            {
                //Hide the highlight quest button
                highlightQuestButton.SetActive(false);
            }
            //Otherwise, show it
            else
            {
                highlightQuestButton.SetActive(true);
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

    void ClearQuestDisplay()
    {
        //Clears the quest title, description, rewards, and conditions
        questTitleText.text = "";
        questDescriptionText.text = "";
        questRewardText.text = "";
        questConditionsText.text = "";
    }

    public void HighlightQuest()
    {
        //Searches for the quest to highlight
        for (int i = 0; i < sideQuests.Count; i++)
        {
            //If found
            if(questTitleText.text == questManager.activeQuests[i].questTitle)
            {
                //Checks if the quest is already highlighted (it shouldnt be)
                if(questManager.activeQuests[i] != questManager.highlightedQuest)
                {
                    //Highlights the quest
                    questManager.highlightedQuest = questManager.activeQuests[i];
                }
                break;
            }    
        }
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

