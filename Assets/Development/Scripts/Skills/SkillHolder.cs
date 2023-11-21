using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SkillHolder : MonoBehaviour
{
    public Key dashKey;
    public Key signatureKey;

    public List<ButtonControl> buttonList = new List<ButtonControl>();

    ButtonControl attackControl;
    KeyControl dashControl;
    KeyControl signatureControl;

    //List of ability scripts on the gameobject
    public List<Skill> skillList;

    public Skill attackAbility;
    public Skill dashAbility;
    public Skill signatureAbility;

    //Fetches the keyboard/mouse input system
    Keyboard kb;
    Mouse mouse;

    public Sequence abilitySequence;

    // Start is called before the first frame update
    void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        mouse = InputSystem.GetDevice<Mouse>();

        attackControl = mouse.leftButton;
        dashControl = kb.FindKeyOnCurrentKeyboardLayout(dashKey.ToString());
        signatureControl = kb.FindKeyOnCurrentKeyboardLayout(signatureKey.ToString());
        //Adds all available abilities into the list
        foreach (Skill a in gameObject.GetComponents<Skill>())
        {
            skillList.Add(a);
        }

        //Declares abilities
        attackAbility = FindAbility("Attack");
        dashAbility = FindAbility("Dash");
        signatureAbility = FindAbility("Signature");


        //Adds all available controls into the list
        buttonList.Add(attackControl);
        buttonList.Add(dashControl);
        buttonList.Add(signatureControl);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HandleUpdate()
    {
        ButtonCheck();
    }

    /// <summary>
    /// Returns the ability if found
    /// </summary>
    /// <param name="abilityName"></param>
    /// <returns></returns>
    public Skill FindAbility(string abilityName)
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
        for (int i = 0; i < skillList.Count - 1; i++)
        {
            if (buttonList[i].wasPressedThisFrame && !skillList[i].onCooldown && skillList[i].usable)
            {
                ActivateAbility(skillList[i]);
            }
        }
    }


    public void ActivateAbility(Skill a)
    {
        a.inUse = true;
        a.Activate();
    }
}
