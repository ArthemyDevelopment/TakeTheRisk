using UnityEngine;

public class EnemyRangeGizmo : MonoBehaviour
{
    #if UNITY_EDITOR

    public int GizmoSize;
    public Color GizmoColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireSphere(transform.position, GizmoSize);
    }


#endif
}
