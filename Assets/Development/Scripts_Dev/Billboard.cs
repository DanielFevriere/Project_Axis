using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera theCam;

    void Awake()
    {
        theCam = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(theCam.transform);
        transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0f, 0f);
    }
}
