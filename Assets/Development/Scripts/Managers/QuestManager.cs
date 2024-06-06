using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Global static reference
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<QuestManager>();
                return instance;
            }
        }
    }
    #endregion

    public Quest highlightedQuest;
    public List<Quest> activeQuests;

    //Accepts the quest passed in through the function
    public void AcceptQuest(Quest targetQuest)
    {
        //Checks if the quest was accepted
        if(activeQuests.Contains(targetQuest))
        {
            return;
        }

        //Adds the target quest to active quests
        activeQuests.Add(targetQuest);

        //Searches the active quests list for the target quest
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i] == targetQuest)
            {
                //Accepts the quest
                activeQuests[i].isAccepted = true;
            }
        }
    }

    //Checks if the quest passed in has been Completed
    public bool CheckIfQuestComplete(Quest targetQuest)
    {
        //Searches the active quests list for the target quest
        for (int i = 0; i < activeQuests.Count; i++)
        {
            //If it's the target quest
            if (activeQuests[i] == targetQuest)
            {
                //Searches through all the quest conditions
                for (int j = 0; j < activeQuests[i].conditions.Count; j++)
                {
                    //If the quest condition is not complete, then the quest is not complete
                    if(!activeQuests[i].conditions[j].Completed)
                    {
                        activeQuests[i].isComplete = false;
                        return activeQuests[i].isComplete;
                    }
                }

                //If it makes it through the search, then the quest is complete
                activeQuests[i].isComplete = true;

                //Returns if the quest is complete or not
                return activeQuests[i].isComplete;
            }
        }

        //if it hits this return, theres an error
        return false;
    }

    //Completes quest
    public void FinishQuest(Quest targetQuest)
    {
        //Checks if the quest was even accepted
        if (!activeQuests.Contains(targetQuest))
        {
            return;
        }

        //Searches the active quests list for the target quest
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i] == targetQuest)
            {
                activeQuests[i].CompleteQuest();
                activeQuests.Remove(activeQuests[i]);
            }
        }

    }

    public void AbandonQuest()
    {

    }

}
