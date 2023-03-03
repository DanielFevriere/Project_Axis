using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class AbilityHolder : MonoBehaviour
{
    public Key dashKey;
    public Key attackKey;
    public Key signatureKey;

    KeyControl dashKeyControl;
    KeyControl attackKeyControl;
    KeyControl signatureKeyControl;

    //List of ability scripts on the gameobject
    public List<Ability> abilityList;

    public Ability dashAbility;
    public Ability attackAbility;
    public Ability signatureAbility;

    //Fetches the keyboard input system
    Keyboard kb;

    // Start is called before the first frame update
    void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        dashKeyControl = kb.FindKeyOnCurrentKeyboardLayout(dashKey.ToString());
        attackKeyControl = kb.FindKeyOnCurrentKeyboardLayout(attackKey.ToString());
        signatureKeyControl = kb.FindKeyOnCurrentKeyboardLayout(signatureKey.ToString());

        //Adds all available abilities into the list
        foreach (Ability a in gameObject.GetComponents<Ability>())
        {
            abilityList.Add(a);
        }

        dashAbility = FindAbility("Dash");
        attackAbility = FindAbility("Attack");
        signatureAbility = FindAbility("Signature");
    }

    // Update is called once per frame
    void Update()
    {
        if(dashKeyControl.wasPressedThisFrame && !dashAbility.onCooldown && dashAbility.enabled)
        {
            dashAbility.Activate();
            DOTween.Sequence()
                .AppendInterval(dashAbility.cooldownTime)
                .AppendCallback(() =>
                {
                    dashAbility.onCooldown = false;
                });
        }

        if (attackKeyControl.wasPressedThisFrame && !attackAbility.onCooldown && attackAbility.enabled)
        {
            attackAbility.Activate();
            DOTween.Sequence()
                .AppendInterval(attackAbility.cooldownTime)
                .AppendCallback(() =>
                {
                    attackAbility.onCooldown = false;
                });
        }

        if (signatureKeyControl.wasPressedThisFrame && !signatureAbility.onCooldown && signatureAbility.enabled)
        {
            signatureAbility.Activate();
            DOTween.Sequence()
                .AppendInterval(signatureAbility.cooldownTime)
                .AppendCallback(() =>
                {
                    signatureAbility.onCooldown = false;
                });
        }
    }

    /// <summary>
    /// Returns the ability if found
    /// </summary>
    /// <param name="abilityName"></param>
    /// <returns></returns>
    public Ability FindAbility(string abilityName)
    {
        //Cycles through list of abilities held
        for (int i = 0; i < abilityList.Count; i++)
        {
            if(abilityList[i].name == abilityName)
            {
                return abilityList[i];
            }
        }

        return null;
    }
}
