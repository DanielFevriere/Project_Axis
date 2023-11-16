using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nodes/AbilityNode")]

public class SkillNode : Node
{
    public Skill ability;

    public override void AcquireNode(GameObject character)
    {
        character.GetComponentInChildren<SkillHolder>().skillList.Add(ability);
    }
}
