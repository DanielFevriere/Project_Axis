using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition
{
    [SerializeField] string conditionName;
    [SerializeField] string conditionID;
    [SerializeField] bool locked;
    [SerializeField] bool completed;
    [SerializeField] int currentProgress;
    [SerializeField] int requiredProgress;

    //Getters
    public string ConditionName
    {
        get { return conditionName; }
    }
    public string ConditionID
    {
        get { return conditionID; }
    }
    public bool Locked
    {
        get { return locked; }
    }
    public bool Completed
    {
        get { return completed; }
    }
    public int CurrentProgress
    {
        get { return currentProgress; }
    }
    public int RequiredProgress
    {
        get { return requiredProgress; }
    }

    //Gives progress
    public void GiveProgress(int amount)
    {
        currentProgress += amount;
        if(currentProgress >= requiredProgress)
        {
            CompleteCondition();
        }
    }

    //Completes Quest Condition
    public void CompleteCondition()
    {
        completed = true;
    }
}

public enum ConditionType
{
    Kill,
    Gather,
    Talk
}
