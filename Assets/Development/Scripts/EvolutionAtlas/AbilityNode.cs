using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nodes/AbilityNode")]

public class AbilityNode : Node
{
    public Ability ability;

    public override void AcquireNode(GameObject character)
    {
        character.GetComponentInChildren<AbilityHolder>().abilityList.Add(ability);
    }
}
