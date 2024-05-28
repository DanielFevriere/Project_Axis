using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nodes/AbilityNode")]

public class SkillNode : Node
{
    public override void UnlockNode()
    {
        base.UnlockNode();
    }
    /// <summary>
    /// Grabs the passed in characters skillholder, and increases it by the nodes amount
    /// </summary>
    public override void ApplyNode(GameObject character)
    {
        character.GetComponentInChildren<SkillHolder>().FindSkill(nodeID).usable = true;
        base.ApplyNode(character);
    }

    public override void UnApplyNode(GameObject character)
    {
        character.GetComponentInChildren<SkillHolder>().FindSkill(nodeID).usable = false;
        base.UnApplyNode(character);
    }
}
