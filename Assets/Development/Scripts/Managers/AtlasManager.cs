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

    public EvolutionAtlas gabeAtlas;
    public EvolutionAtlas mikeAtlas;
    public EvolutionAtlas raphAtlas;

    /// <summary>
    /// Unlocks the node for the specified character
    /// </summary>
    /// <param name="node"></param>
    public void UnlockNode(string nodeID)
    {

    }

    /// <summary>
    /// Makes sure that the abilities, stats, and other effects are applied
    /// </summary>
    public void UpdateAtlas()
    {

    }
}
