using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public bool pickupable;
    [SerializeField] RewardGiver itemPickup;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Make sure to eventually add UI notif feedback
            NotifManager.Instance.QueueNotif(itemPickup);

            itemPickup.GiveReward();
            Destroy(gameObject);
        }
    }
}
