using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsSizeGizmo : MonoBehaviour
{
#if UNITY_EDITOR

    public MeshFilter GizmoSize;
    public Color GizmoColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireMesh(GizmoSize.sharedMesh, transform.position);
    }


#endif
    
}
