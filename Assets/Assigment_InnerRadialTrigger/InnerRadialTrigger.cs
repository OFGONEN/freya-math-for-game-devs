using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class InnerRadialTrigger : MonoBehaviour
{
    public float radiusOuter;
    public float radiusInner;
    public float height;
    [UnityEngine.Range(5, 360)]
    public float fovAngle;

    public Transform turret_head;
    public Transform turret_target;

    private float FovRad => fovAngle * Mathf.Deg2Rad;
    private float AngThresh => Mathf.Cos(FovRad / 2);
    
    public bool isInHeight;
    public bool isInAngle;
    public bool isInRadius;

    public void OnDrawGizmos()
    {
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
        Gizmos.color = Handles.color = Contains(turret_target.position) ? Color.white : Color.red;

        Vector3 top = new Vector3(0, height, 0);

        float p = AngThresh;
        float x = Mathf.Sqrt(1 - p * p);

        Vector3 vLeftDir = new Vector3(-x, 0, p);
        Vector3 vRightDir = new Vector3(x, 0, p);
        Vector3 vLeftInner = vLeftDir * radiusInner;
        Vector3 vRightInner = vRightDir * radiusInner;
        Vector3 vLeftOuter = vLeftDir * radiusOuter;
        Vector3 vRightOuter = vRightDir * radiusOuter;
        
        Gizmos.DrawLine(vLeftInner, vLeftOuter);
        Gizmos.DrawLine(vRightInner, vRightOuter);
        Gizmos.DrawLine(vLeftInner + top, vLeftOuter + top);
        Gizmos.DrawLine(vRightInner + top, vRightOuter + top);
        Gizmos.DrawLine(vLeftInner, vLeftInner + top);
        Gizmos.DrawLine(vRightInner, vRightInner + top);
        Gizmos.DrawLine(vLeftOuter, vLeftOuter + top);
        Gizmos.DrawLine(vRightOuter, vRightOuter + top);
        
        Handles.DrawWireArc(default, Vector3.up, vLeftInner, fovAngle, radiusInner);
        Handles.DrawWireArc(top, Vector3.up, vLeftInner, fovAngle, radiusInner);
        Handles.DrawWireArc(default, Vector3.up, vLeftOuter, fovAngle, radiusOuter);
        Handles.DrawWireArc(top, Vector3.up, vLeftOuter, fovAngle, radiusOuter);
    }

    bool Contains(Vector3 targetPosition)
    {
        var targetLocalPosition = transform.InverseTransformPoint(targetPosition);
        //height check
        if (targetLocalPosition.y < 0 || targetLocalPosition.y > height)
            return false;
        
        //angular check
        Vector3 flatDirToTarget = targetLocalPosition;
        flatDirToTarget.y = 0;
        float flatDistance = flatDirToTarget.magnitude;
        flatDirToTarget = flatDirToTarget.normalized;

        if (Vector3.Dot(Vector3.forward, flatDirToTarget) < AngThresh)
            return false;

        if (flatDistance < radiusInner || flatDistance > radiusOuter)
            return false;
        
        return true;
    }
}
