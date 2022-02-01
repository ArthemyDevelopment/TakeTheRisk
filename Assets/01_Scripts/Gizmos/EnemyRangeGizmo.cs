using System;
using UnityEngine;

public class EnemyRangeGizmo : MonoBehaviour
{
    #if UNITY_EDITOR

    public enum typeColliders
    {
        circle,
        box
    }

    public typeColliders Type_Colliders;
    public Collider col;
    public Color GizmoColor;
    private SphereCollider sc;
    private BoxCollider bc;
    
    private void OnDrawGizmos()
    {
        if (col.GetType() == typeof(SphereCollider))
        {
            sc = (SphereCollider)col;
        }
        else if (col.GetType() == typeof(BoxCollider))
        {
            bc = (BoxCollider)col;
        }

        switch (Type_Colliders)
        {
            case typeColliders.circle:
                Gizmos.color = GizmoColor;
                Gizmos.DrawWireSphere(transform.position,sc.radius);
                break;

            case typeColliders.box:
                Gizmos.color = GizmoColor;
                Gizmos.DrawWireCube(transform.position,bc.size);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


#endif
}
