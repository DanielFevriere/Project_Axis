using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnTouch : MonoBehaviour
{
    public float knockbackAmount;
    public float knockbackDuration;

    [SerializeField] LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // Knocks back if the collider has a component to knock them back
        Knockback knockbackComponent = other.GetComponent<Knockback>();
        if (((1 << other.gameObject.layer) & targetLayer) != 0) //If the collided objects layer is the target layer
        {
            knockbackComponent.ApplyKnockback(gameObject.transform, knockbackAmount, knockbackDuration);
        }
    }
}
