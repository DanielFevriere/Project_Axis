using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SkillHolder : MonoBehaviour
{
    public Key dashKey;
    public Key signature1Key;
    public Key teamSkillKey;
    public Key healSkillKey;

    public List<ButtonControl> buttonList = new List<ButtonControl>();

    ButtonControl attackControl;
    KeyControl dashControl;
    KeyControl signature1Control;
    KeyControl teamSkillControl;
    KeyControl healSkillControl;

    //List of ability scripts on the gameobject
    public List<Skill> skillList;

    public Skill attackSkill;
    public Skill dashSkill;
    public Skill signatureSkill1;
    public Skill teamSkill;
    public Skill healSkill;

    //Fetches the keyboard/mouse input system
    Keyboard kb;
    Mouse mouse;

    // Start is called before the first frame update
    void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        mouse = InputSystem.GetDevice<Mouse>();

        attackControl = mouse.leftButton;
        dashControl = kb.FindKeyOnCurrentKeyboardLayout(dashKey.ToString());
        signature1Control = kb.FindKeyOnCurrentKeyboardLayout(signature1Key.ToString());
        teamSkillControl = kb.FindKeyOnCurrentKeyboardLayout(teamSkillKey.ToString());
        healSkillControl = kb.FindKeyOnCurrentKeyboardLayout(healSkillKey.ToString());


        //Adds all available abilities into the list
        foreach (Skill a in gameObject.GetComponents<Skill>())
        {
            skillList.Add(a);
        }

        //Declares abilities
        attackSkill = FindSkill("Attack");
        dashSkill = FindSkill("Dash");
        signatureSkill1 = FindSkill("Iron Sting");

        //Adds all available controls into the list
        buttonList.Add(attackControl);
        buttonList.Add(dashControl);
        buttonList.Add(signature1Control);
        buttonList.Add(teamSkillControl);
        buttonList.Add(healSkillControl);
    }

    public void HandleUpdate()
    {
        if(!GetComponentInParent<Player>().dead)
        {
            ButtonCheck();
        }
    }

    /// <summary>
    /// Returns the ability if found
    /// </summary>
    /// <param name="abilityName"></param>
    /// <returns></returns>
    public Skill FindSkill(string abilityName)
    {
        //Cycles through list of abilities held
        for (int i = 0; i < skillList.Count; i++)
        {
            if(skillList[i].skillName == abilityName)
            {
                return skillList[i];
            }
        }

        return null;
    }

    public void ButtonCheck()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (buttonList[i].wasPressedThisFrame && !skillList[i].onCooldown && skillList[i].usable && !GetComponentInParent<Player>().stunned)
            {
                ActivateSkill(skillList[i]);
            }
        }
    }
    public void ActivateSkill(Skill a)
    {
        a.inUse = true;
        a.Activate();
    }

    public void EquipSkill(Skill s)
    {
        signatureSkill1 = s;
    }

    public void EquipTeamSkill(Skill s)
    {
        teamSkill = s;
    }
}
