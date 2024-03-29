using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation")]
public class Conversation : ScriptableObject
{
    public string convoTitle;
    public List<Dialogue> Dialogues;
}
