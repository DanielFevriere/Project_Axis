using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    #region Properties

    Rigidbody rb;

    [SerializeField] float lifeTime = 5f;
    [SerializeField] float damageDealt = 1;
    
    #endregion Properties

    void Awake()
    {
        ProjectileManager.Instance.activeProjectiles.Add(this);

        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Sets expiration lifetime
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
    }

    public virtual void LaunchProjectile(Vector3 direction, float force)
    {
        rb.AddForce(direction * force);
    }

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
        
        // Destroy self if collided with something else
        Destroy(gameObject);
    }
    
}
