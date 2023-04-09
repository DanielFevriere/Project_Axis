using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicAsset
{
    public string ID;
    public AudioClip Track;

    public bool Looping;
    // Song only loops between these 2 control points after first loop
    public bool UseLoopPoints;
    // Loop start (in seconds)
    public float LoopStart;
    // Loop end (in seconds)
    public float LoopEnd;
}
