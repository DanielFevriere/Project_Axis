using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{

    // What projectile to launch
    [SerializeField] GameObject projectile;
    // Where to launch projectile from
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float projectileLaunchForce;
    [SerializeField] float cooldown = 1.5f;
    
    float launchTimer = 0f;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        launchTimer += Time.deltaTime;
        if (launchTimer >= cooldown)
        {
            launchTimer = 0f;
            LaunchProjectile();
        }
    }

    public virtual void LaunchProjectile()
    {
        // Play animation etc...
        
        // Compute launch direction
        Vector3 direction = transform.forward;

        // Launch projectile
        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawnPoint.position, quaternion.identity);
        ProjectileBase newProjectile = spawnedProjectile.GetComponent<ProjectileBase>();
        newProjectile.transform.rotation = transform.rotation;
        if (newProjectile)
        {
            newProjectile.LaunchProjectile(direction, projectileLaunchForce);
        }
    }
}
