using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretLookSample : MonoBehaviour
{
    public Transform target;
    public Transform head;

    private void OnDrawGizmos()
    {
        var offset = target.position - head.position;
        Handles.DrawLine(head.position, head.position + offset );

        var localPosition = head.InverseTransformPoint(target.position);
        localPosition.y = 0;

        Handles.color = Color.blue;
        Handles.DrawLine(head.position, head.position + head.TransformPoint(localPosition));
        head.rotation = Quaternion.LookRotation(head.TransformPoint(localPosition) - head.position, transform.up);
        
        Handles.color = Color.yellow;
        Handles.DrawLine(head.position + head.right + head.forward, head.position + head.right - head.forward);
        Handles.DrawLine(head.position + head.right + head.forward, head.position - head.right + head.forward);
        Handles.DrawLine(head.position - head.right + head.forward, head.position - head.right - head.forward);
        Handles.DrawLine(head.position - head.right - head.forward, head.position + head.right - head.forward);
    }
}
