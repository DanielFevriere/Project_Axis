using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction : MonoBehaviour
{
    public float favorability = 0;

    public virtual void PerformAction()
    {
        //Override whatever AI Action here
    }

    public virtual void DetermineWeight()
    {
        //Override the weight determination here
    }
}
