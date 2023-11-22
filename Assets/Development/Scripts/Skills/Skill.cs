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
