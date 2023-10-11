using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition
{
    [SerializeField] string conditionName;
    [SerializeField] bool locked;
    [SerializeField] bool accomplished;
    [SerializeField] int currentProgress;
    [SerializeField] int requiredProgress;
    public string ConditionName
    {
        get { return conditionName; }
    }
    public bool Locked
    {
        get { return locked; }
    }
    public bool Accomplished
    {
        get { return accomplished; }
    }
    public int CurrentProgress
    {
        get { return currentProgress; }
    }
    public int RequiredProgress
    {
        get { return requiredProgress; }
    }

    //Completes Quest Condition
    public virtual void CompleteCondition()
    {
        accomplished = true;
    }
}
