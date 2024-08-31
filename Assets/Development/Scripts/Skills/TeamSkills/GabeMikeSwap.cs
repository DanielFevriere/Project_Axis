using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabeMikeSwap : TeamSkill
{
    public Transform gabePosition;
    public Transform mikePosition;

    GameObject gabe;
    GameObject mike;

    public GameObject hitBox;
    public ParticleSystem effect;

    public float swapDamage;
    public float skillTimer;

    //Goal: Spawn a dynamically sized hitbox between the two of them no matter the distance and swap their positions flawlessly

    // Start is called before the first frame update
    void Awake()
    {
        //Finds and sets the transform reference of mike and gabe
        for (int i = 0; i < GameManager.Instance.partyMembers.Count; i++)
        {
            if(GameManager.Instance.partyMembers[i].GetComponent<Controller>().name == "Gabriel")
            {
                gabe = GameManager.Instance.partyMembers[i];
                gabePosition = gabe.transform;
            }
            if(GameManager.Instance.partyMembers[i].GetComponent<Controller>().name == "Michael")
            {
                mike = GameManager.Instance.partyMembers[i];
                mikePosition = mike.transform;
            }
        }
        skillTimer = skillTime;
    }

    private void Update()
    {
        //Deactivates skill once its done being used
        if(inUse)
        {
            skillTimer -= skillTime;
        }

        if(skillTimer <= 0)
        {
            skillTimer = skillTime;
            Deactivate();
        }
    }

    public override void Activate()
    {
        gabe.GetComponent<CharacterController>().enabled = false;
        SwapLocation();

        SpawnHitBox();
        gabe.GetComponent<CharacterController>().enabled = true;
    }

    void SwapLocation()
    {
        //Switches their locations
        Vector3 newGabePosition = new Vector3(mikePosition.position.x, mikePosition.position.y, mikePosition.position.z);
        Vector3 newMikePosition = new Vector3(gabePosition.position.x, gabePosition.position.y, gabePosition.position.z);

        //Finds and sets the transform reference of mike and gabe
        for (int i = 0; i < GameManager.Instance.partyMembers.Count; i++)
        {
            switchpositions(i, ref newGabePosition, ref newMikePosition);
        }

    }

    private void switchpositions(int i, ref Vector3 newGabePosition, ref Vector3 newMikePosition)
    {
        switch (GameManager.Instance.partyMembers[i].GetComponent<Controller>().name)
        {
            case ("Gabriel"):
                {
                    gabe.transform.position = newGabePosition;
                    break;
                }

            case ("Michael"):
                {
                    mike.transform.position = newMikePosition;
                    break;
                }
        }
    }

    void SwapObjects(GameObject objectA, GameObject objectB)
    {
        Vector3 posA = objectA.gameObject.transform.position;
        Vector3 posB = objectB.gameObject.transform.position;
        objectA.gameObject.transform.position = posB;
        objectB.gameObject.transform.position = posA;
    }

    void SpawnHitBox()
    {
        //Spawns the hitbox between them
        //creates a midpoint vector3
        Vector3 midPoint = (gabePosition.position + mikePosition.position) / 2;

        // Calculate the direction and distance
        Vector3 direction = mikePosition.position - gabePosition.position;
        float distance = direction.magnitude;

        // Instantiate the damage box prefab
        GameObject damageBox = Instantiate(hitBox, midPoint, Quaternion.identity);

        // Adjust the damage box's orientation
        damageBox.transform.forward = direction.normalized;

        // Adjust the size of the box collider
        BoxCollider boxCollider = damageBox.GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            // Assuming the box's Z-axis is the forward direction
            //boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, distance);
            damageBox.transform.localScale = new Vector3(boxCollider.size.x, boxCollider.size.y, distance);
            boxCollider.center = Vector3.zero; // Ensure it's centered
        }

        ParticleSystem p = Instantiate(effect, boxCollider.transform.position, boxCollider.transform.rotation);
        p.transform.localScale = new Vector3(distance, 1, 1);
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
