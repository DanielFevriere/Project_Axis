using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nodes/SpecialNode")]

//Special nodes are Unique and will require a separate class PER special node. Will elaborate on further soon.

public class SpecialNode : Node
{
    public Condition specialCondition;
}
