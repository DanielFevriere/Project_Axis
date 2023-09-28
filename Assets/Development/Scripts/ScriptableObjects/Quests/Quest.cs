using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest")]
public class Quest : ScriptableObject
{
    //Text for the Quest Title, Description, and Objective
    public string questTitle; //Title
    public string questDescription; //Description
    public string questObjective; //Objective

    //Quest States
    public bool isAccepted; // has the quest begun?
    public bool isComplete; // is the quest complete?

    public RewardGiver questReward; //The reward for the quest
    public List<QuestCondition> conditions; //The conditions for this quest

    public void StartQuest()
    {

    }

    public void EndQuest()
    {

    }

    public void CompleteQuest()
    {
        questReward.GiveReward();
    }
}
