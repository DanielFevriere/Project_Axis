using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaphGabeSwap : TeamSkill
{
    public Transform raphPosition;
    public Transform gabePosition;

    GameObject raph;
    GameObject gabe;

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
            if (GameManager.Instance.partyMembers[i].GetComponent<Controller>().name == "Raphael")
            {
                raph = GameManager.Instance.partyMembers[i];
                raphPosition = raph.transform;
            }
            if (GameManager.Instance.partyMembers[i].GetComponent<Controller>().name == "Gabriel")
            {
                gabe = GameManager.Instance.partyMembers[i];
                gabePosition = gabe.transform;
            }
        }
        skillTimer = skillTime;
    }

    private void Update()
    {
        //Deactivates skill once its done being used
        if (inUse)
        {
            skillTimer -= skillTime;
        }

        if (skillTimer <= 0)
        {
            skillTimer = skillTime;
            Deactivate();
        }
    }

    public override void Activate()
    {
        if (GameManager.Instance.currentTP >= tpCost)
        {
            GameManager.Instance.RemoveTP(tpCost);

            raph.GetComponent<CharacterController>().enabled = false;
            SwapObjects(raphPosition.gameObject, gabePosition.gameObject);

            SpawnHitBox();
            raph.GetComponent<CharacterController>().enabled = true;
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
        Vector3 midPoint = (raphPosition.position + gabePosition.position) / 2;

        // Calculate the direction and distance
        Vector3 direction = gabePosition.position - raphPosition.position;
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
