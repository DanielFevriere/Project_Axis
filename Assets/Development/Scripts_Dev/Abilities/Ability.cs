using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public bool enabled;
    public string name;
    public bool inUse;
    public float abilityTime;
    public bool onCooldown;
    public float cooldownTime;

    public virtual void Activate()
    {

    }
}
