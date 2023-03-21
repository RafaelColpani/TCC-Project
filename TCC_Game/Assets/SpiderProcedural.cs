using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectTargets
{
    public Transform localTarget;
    public Transform worldTarget;
}

public class SpiderProcedural : MonoBehaviour
{
    [SerializeField] private List<ObjectTargets> targets;
    [SerializeField] private float hoverRayDistance;

    private void LateUpdate()
    {
        RaycastHit2D hit;

        foreach (var target in targets)
        {
            hit = Physics2D.Raycast(target.localTarget.position, -Vector2.up);

            if (hit.collider == null)
                continue;

            Vector3 rayPoint = hit.point;
            rayPoint.y += hoverRayDistance;
            target.localTarget.position = rayPoint;
        }
    }

    #region GIZMO
    private void OnDrawGizmos()
    {
        
    }
    #endregion
}
