using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public Sprite skillIcon;
    public bool usable;
    public string skillName;
    public string skillDescription;
    public bool inUse;
    public float skillTime;
    public bool onCooldown;
    public float cooldownTime;
    public float cooldownTimer;

    private void FixedUpdate()
    {
        if(onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if(cooldownTimer <= 0)
        {
            onCooldown = false;
        }
    }

    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {
        inUse = false;
        cooldownTimer = cooldownTime;
        onCooldown = true;
    }
}
