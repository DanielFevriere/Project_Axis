using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public bool usable;
    public string skillName;
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
