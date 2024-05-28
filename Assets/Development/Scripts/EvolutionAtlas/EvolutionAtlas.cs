using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Atlas")]
public class EvolutionAtlas : ScriptableObject
{
    public string characterID;
    public GameObject characterObject;

    public List<Node> lockedNodes;
    public List<Node> unlockedNodes;

    public void ApplyNodes()
    {
        //Goes through all unlocked nodes
        for (int i = 0; i < unlockedNodes.Count; i++)
        {
            //If the unlocked nodes haven't been applied yet,
            if (unlockedNodes[i].applied != true)
            {
                //apply them to the respective character
                unlockedNodes[i].ApplyNode(characterObject);
            }
        }
    }

    public void UnApplyNodes()
    {
        //Goes through all unlocked nodes
        for (int i = 0; i < unlockedNodes.Count; i++)
        {
            //If the unlocked nodes haven't been applied yet,
            if (unlockedNodes[i].applied == true)
            {
                //apply them to the respective character
                unlockedNodes[i].UnApplyNode(characterObject);
            }
        }
    }

}
