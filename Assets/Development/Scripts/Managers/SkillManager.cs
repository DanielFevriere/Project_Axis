using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    #region Global static reference
    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<SkillManager>();
                return instance;
            }
        }
    }
    #endregion
    //Any skill placed in the skill manager should already be unlocked

    //Individual and Team skill lists
    public List<Skill> gabrielUnlockedSkills;
    public List<Skill> michaelUnlockedSkills;
    public List<Skill> raphaelUnlockedSkills;
    public List<Skill> teamSkills;

    public List<Skill> gabrielEquippedSkills;
    public List<Skill> michaelEquippedSkills;
    public List<Skill> raphaelEquippedSkills;

    public List<Skill> equippedTeamSkills;

    public Skill EquipSkill(SkillHolder a, int characterIndex)
    {
        //not final, just so code compiles
        Skill x = new Skill();
        return x;
    }
}
