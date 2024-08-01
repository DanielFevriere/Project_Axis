using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnTouch : MonoBehaviour
{
    public enum KnockbackType {Center, Forward}

    public float knockbackAmount;
    public float knockbackDuration;
    public KnockbackType knockbackType;
    //public Vector3 knockbackDirection; //This will be used for a knockback type i add later on

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
            if(knockbackType == KnockbackType.Center)
            {
                knockbackComponent.ApplyKnockback(gameObject.transform, knockbackAmount, knockbackDuration);
            }
            else if(knockbackType == KnockbackType.Forward)
            {
                knockbackComponent.ApplyForwardKnockback(gameObject.transform.position, knockbackAmount, knockbackDuration);
            }
        }
    }

}
