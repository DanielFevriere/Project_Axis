using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : ScriptableObject
{
    public string nodeID;
    public bool locked;
    public bool applied;
    public string nodeDescription;

    //When a node is Unlocked, it needs to be able to give that node affect to the designated character

    public virtual void UnlockNode()
    {

    }
}
