using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] string nameID;
    [SerializeField] string displayName;
    [SerializeField] string line;

    public string NameID
    {
        get { return nameID; }
    }

    public string DisplayName
    {
        get { return displayName; }
    }

    public string Line
    {
        get { return line; }
    }
}
