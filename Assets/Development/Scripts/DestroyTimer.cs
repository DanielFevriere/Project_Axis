using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] float lifeTime = 5f;

    void Start()
    {
        // Sets expiration lifetime
        Destroy(gameObject, lifeTime);
    }
}
