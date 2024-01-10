using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    HP,
    MaxHP,
    LV,
    MaxSP,
    SP,
    ATK,
    DEF,
    SPD,
    
    COUNT
}

public class Stats : MonoBehaviour
{
    #region Properties
    // Stats profile this unit will use
    public StatsProfile statsProfile;

    public int StartingHP;
    public int StartingSP;
    public int StartingATK;
    public int StartingDEF;
    public int StartingSPD;


    // Current stats of the unit
    [Tooltip("Shows the stats")]
    public int[] currentStats = new int[(int)Stat.COUNT];

    public int LV
    {
        get { return currentStats[(int)Stat.LV]; }
        set { currentStats[(int)Stat.LV] = value; }
    }

    public event Action OnTakeDamage;

    float lastDamageTaken = 0f;
    void TakeDamageDebug()
    {
        Debug.Log(gameObject.name + " took " + lastDamageTaken+ " damage ");
    }
        
    #endregion

    void Start()
    {
        // Set stats
        currentStats[(int)Stat.MaxHP] = StartingHP;
        currentStats[(int)Stat.MaxSP] = StartingSP;
        currentStats[(int)Stat.ATK] = StartingATK;
        currentStats[(int)Stat.DEF] = StartingDEF;
        currentStats[(int)Stat.SPD] = StartingSPD;

        // Load base stats example
        // for (int i = (int)Stat.MaxHP; i < (int)Stat.COUNT; i++)
        // {
        //     if (i != (int)Stat.HP || i != (int)Stat.SP)
        //     {
        //         SetStat((Stat)i, (int)statsProfile.GetStatForLevelFromCurve((Stat)i, LV));
        //     }
        // }
        
        // Set HP, SP to MaxHP, MaxSP
        SetStat(Stat.HP, GetStat(Stat.MaxHP));
        SetStat(Stat.SP, GetStat(Stat.MaxSP));

        OnTakeDamage += TakeDamageDebug;
    }

    /// <summary>
    /// If you want to decrease a stat, the amount should be negative
    /// </summary>
    /// <param name="StatToModify"></param>
    /// <param name="Amount"></param>
    public void ModifyStat(Stat StatToModify, int Amount)
    {
        currentStats[(int)StatToModify] += Amount;

        if (StatToModify == Stat.HP && Amount < 0)
        {
            lastDamageTaken = -Amount;
            OnTakeDamage?.Invoke();
        }
    }
    /// <summary>
    /// Sets a stat to the passed in value
    /// </summary>
    /// <param name="StatToSet"></param>
    /// <param name="Value"></param>
    public void SetStat(Stat StatToSet, int Value)
    {
        currentStats[(int)StatToSet] = Value;
    }
    /// <summary>
    /// Returns an int of the passed in stat
    /// </summary>
    /// <param name="StatToGet"></param>
    /// <returns></returns>
    public int GetStat(Stat StatToGet)
    {
        return currentStats[(int)StatToGet];
    }
}

public static class StatsExtensions
{
    public static int MAX_LEVEL = 50;
}

