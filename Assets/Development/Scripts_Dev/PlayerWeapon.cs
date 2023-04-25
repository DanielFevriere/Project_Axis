using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    #region Properties
    Controller player;

    [SerializeField] Transform weaponStart;
    [SerializeField] Transform weaponEnd;
    [SerializeField] float weaponRadius;

    // Layer mask for things we want weapon to hit
    [SerializeField] LayerMask hitMask;

    //The games camera
    [SerializeField] Camera cam;
    //The current position of the mouse
    Vector3 mousePos;

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
                    knockbackComponent.ApplyKnockback(player.gameObject);
                }
            }
        }
    }

    public void UpdateWeaponRotation()
    {

        Ray camera = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float length;

        if (ground.Raycast(camera, out length))
        {
            Vector3 look = camera.GetPoint(length);
            transform.LookAt(new Vector3(look.x, transform.position.y, look.z));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DrawWireCapsule(weaponStart.position, weaponEnd.position, weaponRadius, 0, Color.red);
    }

    #region Gizmos helper
#if UNITY_EDITOR
    public static void DrawWireCapsule(Vector3 _pos, Vector3 _pos2, float _radius, float _height, Color _color = default)
    {
        if (_color != default) Handles.color = _color;
 
        var forward = _pos2 - _pos;
        var _rot = Quaternion.LookRotation(forward);
        var pointOffset = _radius/2f;
        var length = forward.magnitude;
        var center2 = new Vector3(0f,0,length);
       
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
       
        using (new Handles.DrawingScope(angleMatrix))
        {
            Handles.DrawWireDisc(Vector3.zero, Vector3.forward, _radius);
            Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, _radius);
            Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, _radius);
            Handles.DrawWireDisc(center2, Vector3.forward, _radius);
            Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, _radius);
            Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, _radius);
           
            DrawLine(_radius,0f,length);
            DrawLine(-_radius,0f,length);
            DrawLine(0f,_radius,length);
            DrawLine(0f,-_radius,length);
        }
    }
 
    private static void DrawLine(float arg1,float arg2,float forward)
    {
        Handles.DrawLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward));
    }
#endif
    #endregion
}

