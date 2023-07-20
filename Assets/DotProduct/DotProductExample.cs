using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DotProductExample : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private void OnDrawGizmos()
    {
        var dotProductAB = Vector2.Dot(pointA.position, pointB.position);
        var dotProductBA = Vector2.Dot(pointB.position, pointA.position);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, pointA.position);
        Gizmos.DrawSphere(pointA.position.normalized * dotProductAB, 0.1f);
        Handles.Label(pointA.position.normalized * dotProductAB, "AB: " + dotProductAB);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Vector3.zero, pointB.position);
        Gizmos.DrawSphere(pointB.position.normalized * dotProductBA, 0.1f);
        Handles.Label(pointB.position.normalized * dotProductBA, "BA: " + dotProductBA);
    }
}
