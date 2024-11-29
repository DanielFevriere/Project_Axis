using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdle : AIAction
{
    public override void PerformAction()
    {

    }

    public override void DetermineWeight()
    {
        weight = 50;
    }
}
