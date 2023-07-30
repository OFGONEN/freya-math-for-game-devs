using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class InnerRadialTrigger : MonoBehaviour
{
    public enum Shape
    {
        CylindricalSector,
        Spherical,
        Cone
    }

    public Shape triggerShape;
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

    void SetGizmoMatrix(Matrix4x4 m) => Gizmos.matrix = Handles.matrix = m;
    private void OnDrawGizmos()
    {
        SetGizmoMatrix(transform.localToWorldMatrix);
        
        Gizmos.color = Handles.color = Contains(turret_target.position) ? Color.white : Color.red;

        switch (triggerShape)
        {
            case Shape.CylindricalSector: DrawWedgeGizmo();
                break;
            case Shape.Spherical: DrawSphereGizmo();
                break;
            case Shape.Cone: DrawConeGizmo();
                break;
        }
    }

    public void DrawWedgeGizmo()
    {
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
    
    public void DrawSphereGizmo()
    {
        Gizmos.DrawWireSphere(default, radiusInner);
        Gizmos.DrawWireSphere(default, radiusOuter);
    }

    private Stack<Matrix4x4> matrices = new Stack<Matrix4x4>();

    void PushMatrix() => matrices.Push(Gizmos.matrix);
    void PopMatrix() => SetGizmoMatrix(matrices.Pop());
    
    public void DrawConeGizmo()
    {
        float p = AngThresh;
        float x = Mathf.Sqrt(1 - p * p);

        Vector3 vLeftDir = new Vector3(-x, 0, p);
        Vector3 vRightDir = new Vector3(x, 0, p);
        Vector3 vLeftInner = vLeftDir * radiusInner;
        Vector3 vRightInner = vRightDir * radiusInner;
        Vector3 vLeftOuter = vLeftDir * radiusOuter;
        Vector3 vRightOuter = vRightDir * radiusOuter;

        //arcs
        void DrawFlatWedge()
        {
            Handles.DrawWireArc(default, Vector3.up, vLeftInner, fovAngle, radiusInner);
            Handles.DrawWireArc(default, Vector3.up, vLeftOuter, fovAngle, radiusOuter);
            Gizmos.DrawLine(vLeftInner, vLeftOuter);
            Gizmos.DrawLine(vRightInner, vRightOuter);
        }
        //ring
        void DrawRing(float turretRadius)
        {
            float a = FovRad / 2;
            float distance = turretRadius * Mathf.Cos(a);
            float radius = turretRadius * Mathf.Sin(a);
            
            Handles.DrawWireDisc(Vector3.forward * distance, Vector3.forward, radius);
        }
        
        DrawFlatWedge();
        PushMatrix();
            SetGizmoMatrix(Gizmos.matrix * Matrix4x4.TRS(default, Quaternion.Euler(0,0, 90f) , Vector3.one));
            DrawFlatWedge();
        PopMatrix();
        
        DrawRing(radiusInner);
        DrawRing(radiusOuter);
    }

    bool Contains(Vector3 position)
    {
        switch (triggerShape)
        {
            case Shape.CylindricalSector: return WedgeContains(position); 
            case Shape.Spherical: return SphereContains(position);
            case Shape.Cone: return ConeContains(position);
            default:
                return false;
        }
    }

    bool ConeContains(Vector3 position)
    {
        if (SphereContains(position) == false)
            return false;

        Vector3 dirToTarget = (position - transform.position).normalized;
        float projAngle = Vector3.Dot(transform.forward, dirToTarget);
        
        //Check By Angle
        // return Vector3.Angle(transform.forward, dirToTarget) < fovAngle / 2f;
        
        //Check By Angle Manual
        // float angleRad = Mathf.Acos(Vector3.Dot(transform.forward, dirToTarget));
        // return angleRad < FovRad / 2f;
        
        return projAngle > AngThresh;
    }
    
    bool SphereContains(Vector3 position)
    {
        var distance = Vector3.Distance(transform.position, position);
        return distance >= radiusInner && distance <= radiusOuter;
    }
    
    bool WedgeContains(Vector3 position)
    {
        var vecToTargetWorld = position - transform.position;
        var vecToTarget = transform.InverseTransformVector(vecToTargetWorld);

        //height check
        if (vecToTarget.y < 0 || vecToTarget.y > height)
            return false;
        
        //angular check
        Vector3 flatDirToTarget = vecToTarget;
        flatDirToTarget.y = 0;
        float flatDistance = flatDirToTarget.magnitude;
        flatDirToTarget /= flatDistance;

        if (flatDirToTarget.z < AngThresh)
            return false;

        if (flatDistance < radiusInner || flatDistance > radiusOuter)
            return false;
        
        return true;
    }
}
