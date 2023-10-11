using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Stats/Stats Profile")]
public class StatsProfile : ScriptableObject
{
    [Header("Stats: \n" +
        "0 = LV \n" +
        "1 = EXP \n" +
        "2 = MaxHP \n" +
        "3 = HP \n" +
        "4 = MaxSP \n" +
        "5 = SP \n" +
        "6 = ATK \n" +
        "7 = DEF \n" +
        "8 = SPD \n")]

    public AnimationCurve[] StatsCurve = new AnimationCurve[(int)Stat.COUNT];

    // If animation curve's time scale is by level
    public float GetStatForLevelFromCurve(Stat statType, int level = 0)
    {
        return StatsCurve[(int)statType].Evaluate(level);
    }

    // If animation curve's time scale is 0-1
    public float GetStatForLevelFromCurve_01(Stat statType, int level = 0)
    {
        return StatsCurve[(int)statType].Evaluate(level / (float)StatsExtensions.MAX_LEVEL);
    }
}
