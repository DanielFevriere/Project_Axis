using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nodes/StatNode")]
public class StatNode : Node
{
    public Stat stat;
    public int increaseAmount;

    public override void AcquireNode(GameObject character)
    {
        character.GetComponent<Stats>().ModifyStat(stat, increaseAmount);
    }

    public override bool UnlockRequirements()
    {
        return false;
    }
}
