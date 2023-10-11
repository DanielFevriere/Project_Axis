using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : ScriptableObject
{
    public bool locked;
    public int spCost;
    public string nodeDescription;

    //When a node is acquired, it needs to be able to give that node affect to the designated character
    public virtual void AcquireNode(GameObject character)
    {

    }

    public virtual bool UnlockRequirements()
    {
        return false;
    }
}
