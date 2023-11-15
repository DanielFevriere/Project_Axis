using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerWeapon : MonoBehaviour
{
    #region Properties
    Controller player;

    public float weaponKnockback;
    public Transform weaponStart;
    public Transform weaponEnd;
    [SerializeField] float weaponRadius;

    // Layer mask for things we want weapon to hit
    [SerializeField] LayerMask hitMask;

    //The games camera
    [SerializeField] Camera cam;
    //The current position of the mouse
    Vector3 mousePos;

    public float yawRotation;

    #endregion

    void Start()
    {
        //Sets the camera variable to the camera that is mainly being used.
        cam = Camera.main;

        player = GetComponentInParent<Controller>();
    }

    void Update()
    {
    }

    public void Attack()
    {
        Vector3 start = weaponStart.position;
        Vector3 end = weaponEnd.position;

        // Currently only checks overlap for a single frame
        Collider[] collidedObjects = Physics.OverlapCapsule(start, end, weaponRadius, hitMask);

        foreach (var c in collidedObjects)
        {
            if (c.TryGetComponent(out IDamageable enemy))
            {
                enemy.TakeDamage();

                // Can knockback
                Knockback knockbackComponent = c.GetComponent<Knockback>();
                if (knockbackComponent != null)
                {
                    knockbackComponent.ApplyKnockback(player.gameObject.transform, weaponKnockback);
                }
            }
        }
    }

    public void UpdateWeaponRotation()
    {
        Ray camera = cam.ScreenPointToRay(Input.mousePosition);
        // A single point on the plane where the weapon is positioned (in world-space)
        //  in this case, we just want the height of the weapon
        weaponPlanePoint = transform.position;
        // Weapon's plane normal, determines the plane's rotation
        // *Notes: In theory, this value can be changed to achieve weird planes with can result
        //  in different rotations and what not for an attack... but for now we'll use up vector
        //  as the standard
        planeNormal = Vector3.up;
        
        Plane ground = new Plane(planeNormal, weaponPlanePoint);
        float length;

        if (ground.Raycast(camera, out length))
        {
            Vector3 look = camera.GetPoint(length);
            raycastHit = new Vector3(look.x, transform.position.y, look.z);
            transform.LookAt(raycastHit);

            // Find Yaw rotation angle and update animator based on that
            Quaternion aimRotation = transform.rotation;
            yawRotation = aimRotation.eulerAngles.y;
        }
    }

    Vector3 planeNormal;
    Vector3 weaponPlanePoint;
    Vector3 raycastHit;
    
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
            Handles.color = Color.red;
            Vector3 a = raycastHit;
            Vector3 b = raycastHit + Vector3.up * 1f;
            Handles.DrawAAPolyLine(a, b);

            DrawPlane(transform.position, planeNormal);
    }
#endif

    public void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }

    //     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         DrawWireCapsule(weaponStart.position, weaponEnd.position, weaponRadius, 0, Color.red);
//     }
//     #region Gizmos helper
// #if UNITY_EDITOR
//     public static void DrawWireCapsule(Vector3 _pos, Vector3 _pos2, float _radius, float _height, Color _color = default)
//     {
//         if (_color != default) Handles.color = _color;
//  
//         var forward = _pos2 - _pos;
//         var _rot = Quaternion.LookRotation(forward);
//         var pointOffset = _radius/2f;
//         var length = forward.magnitude;
//         var center2 = new Vector3(0f,0,length);
//        
//         Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
//        
//         using (new Handles.DrawingScope(angleMatrix))
//         {
//             Handles.DrawWireDisc(Vector3.zero, Vector3.forward, _radius);
//             Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, _radius);
//             Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, _radius);
//             Handles.DrawWireDisc(center2, Vector3.forward, _radius);
//             Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, _radius);
//             Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, _radius);
//            
//             DrawLine(_radius,0f,length);
//             DrawLine(-_radius,0f,length);
//             DrawLine(0f,_radius,length);
//             DrawLine(0f,-_radius,length);
//         }
//     }
//  
//     private static void DrawLine(float arg1,float arg2,float forward)
//     {
//         Handles.DrawLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward));
//     }
// #endif
//     #endregion
//     
}

