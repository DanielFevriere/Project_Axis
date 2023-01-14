using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] string name;
    [SerializeField] string line;

    public string Name
    {
        get { return name; }
    }

    public string Line
    {
        get { return line; }
    }
}
