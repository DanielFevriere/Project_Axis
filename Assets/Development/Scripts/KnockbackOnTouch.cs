using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnTouch : MonoBehaviour
{
    public float knockbackAmount;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        // Knocks back if the collider has a component to knock them back
        Knockback knockbackComponent = other.GetComponent<Knockback>();
        if (knockbackComponent != null && other.tag == "Player")
        {
            knockbackComponent.ApplyKnockback(gameObject.transform, knockbackAmount);
        }
    }
}
