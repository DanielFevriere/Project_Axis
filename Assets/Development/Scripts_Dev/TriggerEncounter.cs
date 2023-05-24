using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEncounter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<EncounterManager>().StartEncounter();
        }
    }
}
