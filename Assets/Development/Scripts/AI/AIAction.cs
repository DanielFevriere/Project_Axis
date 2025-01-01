using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction : MonoBehaviour
{
    public float weight = 0;
    protected EnemyAIBrain brain;

    private void Start()
    {
        brain = GetComponent<EnemyAIBrain>();
    }

    private void Update()
    {
    }
    public virtual void PerformAction()
    {
        //Override whatever AI Action here
    }

    public virtual void DetermineWeight()
    {

    }
}
