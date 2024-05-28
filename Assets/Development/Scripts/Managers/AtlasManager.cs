using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlasManager : MonoBehaviour
{
    #region Global static reference
    private static AtlasManager instance;
    public static AtlasManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<AtlasManager>();
                return instance;
            }
        }
    }
    #endregion

    public List<EvolutionAtlas> characterAtlases;

    void Start()
    {
        //Searches through character atlases
        for (int i = 0; i < characterAtlases.Count; i++)
        {
            //Searches through each party member
            for (int j = 0; j < GameManager.Instance.partyMembers.Count; j++)
            {
                //If the character atlas' character ID equals the party members player ID
                if(characterAtlases[i].characterID == GameManager.Instance.partyMembers[j].GetComponent<Player>().playerID)
                {
                    //The atlas' character object is that party member
                    characterAtlases[i].characterObject = GameManager.Instance.partyMembers[j];
                }
            }
        }
    }

    /// <summary>
    /// Unlocks the node for the specified character
    /// </summary>
    /// <param name="node"></param>
    public void UnlockNode(string nodeID)
    {
        //Search for the node in each of the character atlases
        for (int i = 0; i < characterAtlases.Count; i++)
        {
            //Searches through the locked nodes of the character atlas
            for (int j = 0; j < characterAtlases[i].lockedNodes.Count; j++)
            {
                //If it equals that ID, unlock it and exit the function
                if(characterAtlases[i].lockedNodes[j].nodeID == nodeID)
                {
                    characterAtlases[i].lockedNodes[j].UnlockNode(); //Unlocks node
                    characterAtlases[i].unlockedNodes.Add(characterAtlases[i].lockedNodes[j]); //Adds the node to unlocked
                    characterAtlases[i].lockedNodes.Remove(characterAtlases[i].lockedNodes[j]); //Removes the node from locked
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Makes sure that the abilities, stats, and other effects are applied
    /// </summary>
    public void UpdateAtlas()
    {
        //Goes through all the character atlases and applies any node that hasn't been applied yet
        for (int i = 0; i < characterAtlases.Count; i++)
        {
            characterAtlases[i].ApplyNodes();
        }
    }

    /// <summary>
    /// Debug purposes, locks and resets all nodes
    /// </summary>
    public void ResetAtlas()
    {
        for (int i = 0; i < characterAtlases.Count; i++)
        {
            characterAtlases[i].UnApplyNodes();
        }
    }
}
