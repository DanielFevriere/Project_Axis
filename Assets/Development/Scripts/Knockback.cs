using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    #region Properties
    // Rigid body 
    [SerializeField] bool useRigidBody;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] float currentKnockbackDuration;

    public UnityEvent OnKnockBackBegin, OnForceBegin, OnComplete; 
    #endregion

    public void ApplyKnockback(Transform source, float knockbackStrength, float duration)
    {
        currentKnockbackDuration = duration;

        // Stop all previous knocking back coroutines
        StopAllCoroutines();
        // Call OnBegin event
        //      to disable enemy AI movement components and other transforms behavior
        OnKnockBackBegin?.Invoke();
        // Get the direction from source to this unit
        Vector3 dir = (this.transform.position - source.position).normalized;
        // Zero out y because we only care about 2D (x, z)
        dir.y = 0;
        // Turn on rigidbody and physics control for knockback
        rigidBody.isKinematic = false;

        //Resets velocity before adding a new force (problem: if its knocked back again too soon, it will nullify the force)
        //rigidBody.velocity = Vector3.zero; 

        // Add force to rigid body for knockback
        rigidBody.AddForce(dir * knockbackStrength, ForceMode.Impulse);
        //print(dir);
        // Start coroutine to reset force after x amount of time
        StartCoroutine(Reset());
    }

    public void ApplyForwardKnockback(Vector3 source, float knockbackStrength, float duration)
    {
        currentKnockbackDuration = duration;

        // Stop all previous knocking back coroutines
        StopAllCoroutines();
        // Call OnBegin event
        //      to disable enemy AI movement components and other transforms behavior
        OnKnockBackBegin?.Invoke();
        // Turn on rigidbody and physics control for knockback
        rigidBody.isKinematic = false;

        //Resets velocity before adding a new force (problem: if its knocked back again too soon, it will nullify the force)
        //rigidBody.velocity = Vector3.zero; 

        // Add force to rigid body for knockback
        rigidBody.AddRelativeForce(-Vector3.forward * knockbackStrength, ForceMode.Impulse);
        //print(dir);
        // Start coroutine to reset force after x amount of time
        StartCoroutine(Reset());
    }

    /// <summary>
    /// The EXACT same function as ApplyKnockback() but it doesn't put enemies/players into a state of knockback to stun them
    /// </summary>
    /// <param name="source"></param>
    /// <param name="knockbackStrength"></param>
    /// <param name="duration"></param>
    public void ApplyForce(Vector3 source, float knockbackStrength, float duration)
    {
        currentKnockbackDuration = duration;

        // Stop all previous knocking back coroutines
        StopAllCoroutines();
        // Call OnBegin event
        //      to disable enemy AI movement components and other transforms behavior
        OnForceBegin?.Invoke();
        // Get the direction from source to this unit
        Vector3 dir = (this.transform.position - source).normalized;
        // Zero out y because we only care about 2D (x, z)
        dir.y = 0;
        // Turn on rigidbody and physics control for knockback
        rigidBody.isKinematic = false;

        //Resets velocity before adding a new force (problem: if its knocked back again too soon, it will nullify the force)
        rigidBody.velocity = Vector3.zero; 

        // Add force to rigid body for knockback
        rigidBody.AddForce(dir * knockbackStrength, ForceMode.Impulse);
        //print(dir);
        // Start coroutine to reset force after x amount of time
        StartCoroutine(Reset());
    }

    public void ApplyForwardForce(Vector3 source, float knockbackStrength, float duration)
    {
        currentKnockbackDuration = duration;

        // Stop all previous knocking back coroutines
        StopAllCoroutines();
        // Call OnBegin event
        //      to disable enemy AI movement components and other transforms behavior
        OnForceBegin?.Invoke();
        // Turn on rigidbody and physics control for knockback
        rigidBody.isKinematic = false;

        //Resets velocity before adding a new force (problem: if its knocked back again too soon, it will nullify the force)
        rigidBody.velocity = Vector3.zero; 

        // Add force to rigid body for knockback
        rigidBody.AddRelativeForce(source * knockbackStrength, ForceMode.Impulse);
        //print(dir);
        // Start coroutine to reset force after x amount of time
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return null;
        if (useRigidBody)
        {
            // Wait for knockback to finish
            yield return new WaitForSeconds(currentKnockbackDuration);
            // Reset velocity on rigid body
            rigidBody.velocity = Vector3.zero;
            // Turn off rigidbody and physics control, and revert back to animations/movement components
            rigidBody.isKinematic = true;
            // Call OnComplete event
            //      which should trigger re-enabling enemy AI movement components and other transforms behaviors
            OnComplete?.Invoke();
        }
    }
}
