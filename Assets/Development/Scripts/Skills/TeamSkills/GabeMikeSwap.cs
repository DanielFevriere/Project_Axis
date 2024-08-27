using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabeMikeSwap : TeamSkill
{
    public Transform gabePosition;
    public Transform mikePosition;

    public GameObject hitBox;
    public float swapDamage;

    //Goal: Spawn a dynamically sized hitbox between the two of them no matter the distance and swap their positions flawlessly

    // Start is called before the first frame update
    void Awake()
    {
        //Finds and sets the transform reference of mike and gabe
        for (int i = 0; i < GameManager.Instance.partyMembers.Count; i++)
        {
            if(GameManager.Instance.partyMembers[i].GetComponent<Controller>().name == "Gabriel")
            {
                gabePosition = GameManager.Instance.partyMembers[i].transform;
            }
            if(GameManager.Instance.partyMembers[i].GetComponent<Controller>().name == "Michael")
            {
                mikePosition = GameManager.Instance.partyMembers[i].transform;
            }
        }
    }

    public override void Activate()
    {
        //Switches their locations
        Transform newGabePosition = mikePosition;
        Transform newMikePosition = gabePosition;

        gabePosition = newGabePosition;
        mikePosition = newMikePosition;

        //Spawns the hitbox between them
        //creates a midpoint vector3
        Vector3 midPoint = (gabePosition.position + mikePosition.position) / 2;
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
