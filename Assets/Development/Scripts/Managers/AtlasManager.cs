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
    public EvolutionAtlas gabeAtlas;
    public EvolutionAtlas mikeAtlas;
    public EvolutionAtlas raphAtlas;

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
                    characterAtlases[i].lockedNodes[j].UnlockNode();
                    characterAtlases[i].unlockedNodes.Add(characterAtlases[i].lockedNodes[j]);
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
        //Goes through all the character atlases
        for (int i = 0; i < characterAtlases.Count; i++)
        {

        }
    }
}
