using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;

    public float trigger_height = 1f;
    public float trigger_radius = 1f;
    [Range(0, 1)]
    public float trigger_angle_threshold = 0.5f;

    public bool isInHeight, isInRadius, isInAngle;
    
    private void OnDrawGizmos() => OnTriggerWithThreshold();

    private void OnTest()
    {
        var offset = target.position - transform.position;
        Handles.Label(target.position, "Angle Threshold: " + Vector3.Dot(transform.forward, offset.normalized) );
    }
        
    //Assigment 5.B
    private void OnTriggerWithThreshold()
    {
        if(!target) return;

        var forward = transform.forward;
        var right = transform.right;
        var up = transform.up;
        
        var origin = transform.position;
        var top = origin + up * trigger_height;

        var targetInversedPosition = transform.InverseTransformPoint(target.position);
        var targetInversedVector = transform.InverseTransformDirection(target.position - transform.position);
        targetInversedVector.y = 0;
        targetInversedVector = targetInversedVector.normalized;

        isInHeight = targetInversedPosition.y > 0 && targetInversedPosition.y < trigger_height;
        isInRadius =
            (targetInversedPosition.x * targetInversedPosition.x + targetInversedPosition.z * targetInversedPosition.z) <
            trigger_radius * trigger_radius;
        isInAngle = trigger_angle_threshold < targetInversedVector.z;
        bool isTrigger = isInHeight && isInRadius && isInAngle;

        Handles.Label(target.position + Vector3.up / 2f, "Threshold: " + Vector3.Dot(forward, targetInversedPosition.normalized));
        Handles.Label(target.position, "LocalPos: " + targetInversedPosition);
        
        var color = isTrigger ? Color.red : Color.green;

        Handles.color = color;
        Gizmos.color = color;
        
        Handles.DrawWireDisc(origin, up, trigger_radius);
        Handles.DrawWireDisc(top, up, trigger_radius);

        float p = trigger_angle_threshold;
        float x = Mathf.Sqrt(1 - p * p);
        Vector3 vRight = (forward * trigger_angle_threshold + right * x) * trigger_radius;
        Vector3 vLeft = (forward * trigger_angle_threshold - right * x) * trigger_radius;
        
        Gizmos.DrawRay(origin, vRight);
        Gizmos.DrawRay(origin, vLeft);
        Gizmos.DrawRay(top, vRight);
        Gizmos.DrawRay(top, vLeft);
        
        Gizmos.DrawLine(origin, top);
        Gizmos.DrawLine(origin + vLeft, top + vLeft);
        Gizmos.DrawLine(origin + vRight, top + vRight);
    }
    
    //Assigment 5.A
    private void OnTriggerWithHeigthAndRange()
    {
        if (!target) return;
        
        var offset = transform.InverseTransformPoint(target.position);
                
        bool isTriggered = offset.y > 0 && offset.y < trigger_height && (offset.x * offset.x + offset.z + offset.z) < trigger_radius* trigger_radius;
        
        Handles.color = isTriggered ? Color.red : Color.green;
        Handles.DrawWireArc(transform.position, transform.up, -transform.right, 360f, trigger_radius);
        Handles.DrawWireArc(transform.position + transform.up * trigger_height, transform.up, -transform.right, 360f, trigger_radius);
                
        Handles.DrawLine(transform.position + -transform.right * trigger_radius, transform.position + -transform.right * trigger_radius + transform.up * trigger_height);
        Handles.DrawLine(transform.position + transform.forward * trigger_radius, transform.position + transform.forward * trigger_radius + transform.up * trigger_height);
        Handles.DrawLine(transform.position + transform.right * trigger_radius, transform.position + transform.right * trigger_radius + transform.up * trigger_height);
        Handles.DrawLine(transform.position + -transform.forward * trigger_radius, transform.position + -transform.forward * trigger_radius + transform.up * trigger_height);
    }
}
