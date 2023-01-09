using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] string name;
    [SerializeField] List<string> lines;

    public string Name
    {
        get { return name; }
    }

    public List<string> Lines
    {
        get { return lines; }
    }
}
