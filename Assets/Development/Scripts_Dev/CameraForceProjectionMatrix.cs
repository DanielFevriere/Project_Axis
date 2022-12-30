using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraForceProjectionMatrix : MonoBehaviour
{
    public Vector4 Up = new Vector4(0, 1, 0, 0);
    Camera MainCamera;

    private void Awake()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();   
    }

    private void LateUpdate()
    {
        if (GetComponent<Camera>() != MainCamera)
        {
            return;
        }

        // First calculate the regular worldToCameraMatrix.
        // Start with transform.worldToLocalMatrix.
 
        var m = GetComponent<Camera>().transform.worldToLocalMatrix;
        // Then, since Unity uses OpenGL's view matrix conventions
        // we have to flip the z-value.
        m.SetRow(2, -m.GetRow(2));
 
        // Now for the custom projection.
        // Set the world's up vector to always align with the camera's up vector.
        // Add a small amount of the original up vector to
        // ensure the matrix will be invertible.
        // Try changing the vector to see what other projections you can get.
        m.SetColumn(1, 1e-3f * m.GetColumn(1) + Up);
 
        GetComponent<Camera>().worldToCameraMatrix = m;
    }
}