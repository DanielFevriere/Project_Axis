using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    LV,
    EXP,
    MaxHP,
    HP,
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

        public int StartingLV;
        public int StartingEXP;
        
        // Current stats of the unit
        [Tooltip("Shows the stats")]
        public int[] currentStats = new int[(int)Stat.COUNT];

        public int LV
        {
            get { return currentStats[(int)Stat.LV]; }
            set { currentStats[(int)Stat.LV] = value; }
        }
        
    #endregion

    void Start()
    {
        // Set level and exp
        LV = StartingLV;
        currentStats[(int)Stat.EXP] = StartingEXP;
        
        // Load base stats example
        for (int i = (int)Stat.MaxHP; i < (int)Stat.COUNT; i++)
        {
            if (i != (int)Stat.HP || i != (int)Stat.SP)
            {
                SetStat((Stat)i, (int)statsProfile.GetStatForLevelFromCurve((Stat)i, LV));
            }
        }
        
        // Set HP, SP to MaxHP, MaxSP
        SetStat(Stat.HP, GetStat(Stat.MaxHP));
        SetStat(Stat.SP, GetStat(Stat.MaxSP));
    }

    public void ModifyStat(Stat StatToModify, int Amount)
    {
        currentStats[(int)StatToModify] -= Amount;
    }

    public void SetStat(Stat StatToSet, int Value)
    {
        currentStats[(int)StatToSet] = Value;
    }

    public int GetStat(Stat StatToGet)
    {
        return currentStats[(int)StatToGet];
    }
}

public static class StatsExtensions
{
    public static int MAX_LEVEL = 50;
}

