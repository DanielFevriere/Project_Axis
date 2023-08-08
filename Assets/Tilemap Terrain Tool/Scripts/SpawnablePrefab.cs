#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnablePrefab : MonoBehaviour
{
    public float width = 0.5f;
    public float height = 1f;

    public Vector3 Size
    {
        get
        {
            return new Vector3(
                width * transform.localScale.x, 
                height * transform.localScale.y,
                width * transform.localScale.z);
        }
    }

    public Vector3 Center
    {
        get
        {
            return transform.position + transform.up * Size.y * 0.5f;;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 a = transform.position;
        Vector3 b = transform.position + transform.up * height;
        
        //Handles.DrawAAPolyLine(a, b);

        Handles.DrawWireCube(Center, Size);
        
        // Local function
        void DrawSphere(Vector3 p) => Gizmos.DrawSphere(p, HandleUtility.GetHandleSize(p) * 0.3f);

        DrawSphere(a);
        DrawSphere(b);
    }
}
#endif