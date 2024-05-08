using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public float damageDealt;
    [SerializeField] LayerMask targetLayer;

    // Interaction with units and environments
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject)
        {
            return;
        }

        Debug.Log("Hitbox collided with " + other.gameObject.name);

        // Deals damage if collided with targetLayer
        if (((1 << other.gameObject.layer) & targetLayer) != 0) //If the collided objects layer is the target layer
        {
            if (other.TryGetComponent(out IDamageable entity))
            {
                 entity.TakeDamage(damageDealt);
            }
        }
    }

}
