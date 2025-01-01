using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdle : AIAction
{
    private void Start()
    {
        brain = GetComponent<EnemyAIBrain>();
    }
    private void Update()
    {

    }
    public override void PerformAction()
    {

    }

    public override void DetermineWeight()
    {
        if (brain.playerInSightRange)
        {
            weight = 0;
        }
        else if (!brain.playerInSightRange)
        {
            weight = 0;
        }

    }
}
