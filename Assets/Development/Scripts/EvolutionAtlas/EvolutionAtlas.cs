using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Atlas")]
public class EvolutionAtlas : ScriptableObject
{
    public GameObject character;

    public List<Node> lockedNodes;
    public List<Node> unlockedNodes;
    public List<Node> acquiredNodes;

}
