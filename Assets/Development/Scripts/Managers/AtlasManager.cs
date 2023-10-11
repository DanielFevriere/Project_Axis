using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlasManager : MonoBehaviour
{
    public EvolutionAtlas gabeAtlas;
    public EvolutionAtlas mikeAtlas;
    public EvolutionAtlas raphAtlas;

    /// <summary>
    /// Gives SP to the character with the passed in name
    /// </summary>
    /// <param name="character"></param>
    public void GiveSP(string character, int amount)
    {
        //Goes through the party members
        for (int i = 0; i < GameManager.Instance.partyMembers.Count; i++)
        {
            //finds the specific partymember
            if (character == GameManager.Instance.partyMembers[i].name)
            {
                //gives them sp
                GameManager.Instance.partyMembers[i].GetComponent<Stats>().ModifyStat(Stat.SP, amount);
            }
        }
    }

    /// <summary>
    /// Unlocks the node passed in by the parameter
    /// </summary>
    /// <param name="node"></param>
    public void UnlockNode(Node node)
    {
        //If the nodes unlock requirements return true, unlock the node
        if(node.UnlockRequirements())
        {
            node.locked = false;
        }
        else
        {
            return;
        }    
    }

    /// <summary>
    /// Whatever the node has to provide, it's contents are given to the designated character
    /// </summary>
    /// <param name="node"></param>
    public void AcquireNode(Node node, EvolutionAtlas atlas)
    {
        //Searches for the node
        for (int i = 0; i < atlas.unlockedNodes.Count; i++)
        {
            //if it's the node that's being looked for
            if(atlas.unlockedNodes[i] == node)
            {
                node.AcquireNode(atlas.character);
            }
        }
    }
}
