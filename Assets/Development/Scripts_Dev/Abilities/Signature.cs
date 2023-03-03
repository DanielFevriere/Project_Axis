using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signature : Ability
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void Activate()
    {
        onCooldown = true;
    }
}
