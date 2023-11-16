using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    //Skill Icon
    public Sprite icon;
    public string skillName;
    public string skillDescription;

    public bool usable;
    public bool inUse;
    public float skillTime;
    public bool onCooldown;
    public float cooldownTime;

    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {
        inUse = false;
        onCooldown = true;
        DOTween.Sequence()
            .AppendInterval(cooldownTime)
            .AppendCallback(() =>
            {
                onCooldown = false;
            });
    }
}
