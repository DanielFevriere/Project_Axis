using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    HP = 0,
    COUNT
}

public class Stats : MonoBehaviour
{
    [Tooltip("Shows the stats")]
    public int[] currentStats = new int[(int)Stat.COUNT];

}


