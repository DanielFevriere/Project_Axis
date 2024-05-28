using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nodes/StatNode")]
public class StatNode : Node
{
    public Stat stat;
    public int increaseAmount;

    public override void UnlockNode()
    {
        base.UnlockNode();
    }
    /// <summary>
    /// Grabs the passed in characters stat, and increases it by the nodes amount
    /// </summary>
    public override void ApplyNode(GameObject character)
    {
        character.GetComponent<Stats>().ModifyStat(stat, increaseAmount);
        base.ApplyNode(character);
    }

    public override void UnApplyNode(GameObject character)
    {
        character.GetComponent<Stats>().ModifyStat(stat, -increaseAmount);
        base.UnApplyNode(character);
    }
}
