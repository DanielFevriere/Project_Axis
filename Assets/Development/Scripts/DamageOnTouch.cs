using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public float damageDealt;


    // Interaction with units and environments
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject)
        {
            return;
        }

        Debug.Log("Projectile collided with " + other.gameObject.name);

        // Deals damage if collided with player
        if (other.tag == "Player")
        {
            if (other.TryGetComponent(out IDamageable player))
            {
                player.TakeDamage(damageDealt);
            }
        }
    }
}
