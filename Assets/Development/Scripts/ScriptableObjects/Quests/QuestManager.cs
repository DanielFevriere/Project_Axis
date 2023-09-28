using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests;
    public static QuestManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

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

    //Checks if the quest passed in has been accepted
    public bool CheckIfQuestComplete(Quest targetQuest)
    {
        //Searches the active quests list for the target quest
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i] == targetQuest)
            {
                //Completes the quest
                return activeQuests[i].isComplete;
            }
        }

        //if it hits this return, theres an error
        return false;
    }

    //Completes quest
    public void FinishQuest(Quest targetQuest)
    {
        //Checks if the quest was accepted
        if (!activeQuests.Contains(targetQuest))
        {
            return;
        }

        //Searches the active quests list for the target quest
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i] == targetQuest)
            {
                //Completes the quest
                activeQuests[i].isComplete = true;
                //activeQuests[i].CompleteQuest(); Add functionality for quest rewarding before uncommenting this
                activeQuests.Remove(activeQuests[i]);
            }
        }

    }

    public void AbandonQuest()
    {

    }

}
