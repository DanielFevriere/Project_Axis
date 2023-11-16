using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //Any skill placed in the skill manager should already be unlocked

    //Individual and Team skill lists
    public List<Skill> gabrielIndividualSkills;
    public List<Skill> michaelIndividualSkills;
    public List<Skill> raphaelIndividualSkills;
    public List<Skill> teamAbilities;

    public Skill EquipSkill(SkillHolder a, int characterIndex)
    {
        //not final, just so code compiles
        Skill x = new Skill();
        return x;
    }
}
