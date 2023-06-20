using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitherObjectsBetweenCamAndPlayer : MonoBehaviour
{
    // Stores all hits from previous frame
    RaycastHit[] hits = null;

    public Transform target;
    public LayerMask layerMask;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (target == null)
        {
            return;
        }

        // Refresh all previous objects
        if (hits != null)
        {
            foreach (RaycastHit hit in hits)
            {
                Material material = hit.collider.GetComponent<Renderer>().sharedMaterial;
                material.DisableKeyword("_DITHER");
            }
        }


        // Raycast
        Vector3 dir = (target.transform.position - this.transform.position).normalized;
        hits = Physics.RaycastAll(this.transform.position, dir.normalized, Vector3.Distance(this.transform.position, target.transform.position) - 1.0f, layerMask);
        
        if (hits != null)
        {
            foreach (RaycastHit hit in hits)
            {
                Material material = hit.collider.GetComponent<Renderer>().sharedMaterial;
                material.EnableKeyword("_DITHER");
            }
        }

        // Debug
        Debug.DrawRay(this.transform.position, (target.transform.position - this.transform.position), Color.yellow);
    }
}
