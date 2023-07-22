using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Turret : MonoBehaviour
{
    public Transform target;

    public float trigger_height = 1f;
    public float trigger_radius = 1f;

    private void OnDrawGizmos()
    {
        if (!target) return;

        var offset = transform.InverseTransformPoint(target.position);
        
        bool isTriggered = Mathf.Abs(offset.y) < trigger_height && (offset.x * offset.x + offset.z + offset.z) < trigger_radius* trigger_radius;

        Handles.color = isTriggered ? Color.red : Color.green;
        Handles.DrawWireArc(transform.position, transform.up, -transform.right, 360f, trigger_radius);
        Handles.DrawWireArc(transform.position + transform.up * trigger_height, transform.up, -transform.right, 360f, trigger_radius);
        
        Handles.DrawLine(transform.position + -transform.right * trigger_radius, transform.position + -transform.right * trigger_radius + transform.up * trigger_height);
        Handles.DrawLine(transform.position + transform.forward * trigger_radius, transform.position + transform.forward * trigger_radius + transform.up * trigger_height);
        Handles.DrawLine(transform.position + transform.right * trigger_radius, transform.position + transform.right * trigger_radius + transform.up * trigger_height);
        Handles.DrawLine(transform.position + -transform.forward * trigger_radius, transform.position + -transform.forward * trigger_radius + transform.up * trigger_height);
    }
}
